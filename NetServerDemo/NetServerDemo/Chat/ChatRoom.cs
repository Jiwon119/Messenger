using NetServerDemo.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// 정보를 저장

namespace NetServerDemo.Chat
{

    internal struct RoomMeta
    {
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public HashSet<string> RoomUser { get; set; }
        public string RoomLeader { get; set; }
        public string CreationTime { get; set; }
        public int NewChat { get; set; }
        public DateTime LastChatTime { get; set; }
    }
    internal struct ChatMeta
    {
        public string? UserID { get; set; }
        public string? Content { get; set; }
        public string? CreateTime { get; set; }
    }



    internal class ChatRoom
    {
        private HashSet<string> _myUserList = new HashSet<string>();
        private string _roomLeader = string.Empty;
        private Room _myRoomInfo;
        public DateTime lastChatTime;

        public ChatRoom(Room info)
        {
            _myRoomInfo = info;
            _roomLeader = RoomLeaderSetting(info);
            lastChatTime = LastChatTimeSetting();
            var users = UserListSetting(info);
            foreach (var user in users)
            {
                _myUserList.Add(user);
            }
        }
        // 채팅방의 마지막 대화 시간 받아오기
        public DateTime LastChatTimeSetting()
        {
            using(var db = new DBContext())
            {
                var chat = (from c in db.Chats
                            where c.RoomNo == _myRoomInfo.RoomID
                            select c).ToList();
                if (chat.Count == 0) 
                {
                    return _myRoomInfo.CreateTime;
                }
                else
                {
                    return chat[chat.Count - 1].CreateTime;
                }

                
            }
        }



        // 유저가 있나 체크
        public bool IsValidUser(string userId)
        {
            return _myUserList.Contains(userId);
        }

        #region DB검색
        // DB에서 유저 리스트 검색
        private HashSet<string> UserListSetting(Room info)
        {
            HashSet<string> userList = new HashSet<string>();
            using (var db = new DBContext())
            {
                userList = (from u in db.User_Rooms
                            where u.RoomID == info.RoomID
                            select u.UserID).ToHashSet();
            }
            userList.Remove("sys");
            return userList;
        }

        // 새로운 채팅 개수 setting
        public int NewChatNumSetting(string userId)
        {
            using (var db = new DBContext())
            {
                int chatNum = (from chat in db.User_Chats
                               where chat.RoomID == _myRoomInfo.RoomID && chat.UserID == userId
                               select chat).Count();
                return chatNum;
            }
        }

        // DB에서 채팅방 방장 검색
        private string RoomLeaderSetting(Room info)
        {
            string leader = string.Empty;
            using (var db = new DBContext())
            {
                leader = (from u in db.User_Rooms
                          where u.RoomID == info.RoomID && u.RoomLeader == true
                          select u.UserID).FirstOrDefault()!.ToString()!;
            }
            return leader;
        }

        #endregion

        #region SetRoomInfo

        // set RoomName
        public void SetRoomName(string editedRoomName)
        {
            using (var db = new DBContext())
            {
                // 수정된 채팅방 이름 입력
                var room = db.Rooms!.Find(_myRoomInfo.RoomID);

                // 알림을 주기 위해서 chat 형태로 sys 계정 데이터를 DB에 저장.
                if (_myRoomInfo.RoomName != editedRoomName)
                {
                    //DB에 저장
                    room!.RoomName = editedRoomName;

                    // _myRoomInfo에 저장
                    _myRoomInfo.RoomName = editedRoomName;

                    SaveChatLog("sys", _myRoomInfo.RoomID, $"채팅방 이름이 <{editedRoomName}>으로 변경되었습니다.", DateTime.Now);
                }
                db.SaveChanges();
            }
        }
        public void SetUserList(Array removeUser, Array addUser)
        {
            _myUserList = UserListSetting(_myRoomInfo);
            using (var db = new DBContext())
            {
                // 수정된 유저 입력
                foreach (var a in removeUser!)
                {
                    var remove = (from user in db.User_Rooms
                                  where user.UserID == a.ToString() && user.RoomID == _myRoomInfo.RoomID
                                  select user).FirstOrDefault();
                    db.User_Rooms!.Remove(remove!);

                    SaveChatLog("sys", _myRoomInfo.RoomID, $"{a}님이 강제퇴장되었습니다.", DateTime.Now);
                }

                foreach (var a in addUser!)
                {
                    User_Room user = new User_Room();
                    user.UserID = a.ToString();
                    user.RoomID = _myRoomInfo.RoomID;
                    user.RoomLeader = false;
                    user.CreateTime = DateTime.Now;

                    db.User_Rooms!.Add(user);
                    SaveChatLog("sys", _myRoomInfo.RoomID, $"{a}님이 초대되었습니다.", DateTime.Now);
                }
                db.SaveChanges();
            }
            _myUserList = UserListSetting(_myRoomInfo);
        }

        
        public void SetRoomLeader(string changeLeaderName)
        {
            using(var db = new DBContext())
            {
                if (changeLeaderName != "")
                {
                    // 해당 유저를 방장으로
                    User_Room grantLeader = (from user in db.User_Rooms!
                                             where user.UserID == changeLeaderName && user.RoomID == _myRoomInfo.RoomID
                                             select user).FirstOrDefault()!;
                    grantLeader.RoomLeader = true;

                    // 원래 유저의 방장 권한 박탈
                    User_Room revokeLeader = (from user in db.User_Rooms!
                                              where user.UserID == _roomLeader && user.RoomID == _myRoomInfo.RoomID
                                              select user).FirstOrDefault()!;
                    revokeLeader.RoomLeader = false;

                    SaveChatLog("sys", _myRoomInfo.RoomID, $"방장이 {changeLeaderName}님으로 변경되었습니다.", DateTime.Now);

                    _roomLeader = changeLeaderName;
                }
                db.SaveChanges();
            }
        }
        public void ChatRoomOutUser(string userId)
        {
            using (var db = new DBContext())
            {
                var remove = (from c in db.User_Rooms
                              where c.UserID == userId && c.RoomID == _myRoomInfo.RoomID
                              select c).FirstOrDefault();

                db.User_Rooms!.Remove(remove!);

                db.SaveChanges();
            }
            SaveChatLog("sys", _myRoomInfo.RoomID, $"{userId}님이 채팅방을 나갔습니다.", DateTime.Now);
            _myUserList.Remove(userId);
        }

        #endregion

        #region GetRoomInfo

        // get 채팅방 정보
        public RoomMeta GetMyRoomMeta(string userId)
        {
            int chatNum;
            using (var db = new DBContext())
            {
                chatNum = (from chat in db.User_Chats
                           where chat.RoomID == _myRoomInfo.RoomID && chat.UserID == userId && chat.Check == false
                           select chat).Count();

            }
            return new RoomMeta()
            {
                RoomID = _myRoomInfo.RoomID.ToString(),
                CreationTime = _myRoomInfo.CreateTime!.ToString("yyyy/MM/dd HH:mm")!,
                RoomName = _myRoomInfo.RoomName!,
                RoomUser = _myUserList,
                RoomLeader = _roomLeader,
                LastChatTime = lastChatTime,
                NewChat = chatNum
            };
        }

        // get 대화 정보
        public List<ChatMeta> GetMyChatMeta(string userId)
        {
            List<ChatMeta> Chatlist = new List<ChatMeta>();
            using (var db = new DBContext())
            {
                DateTime UserEnterTime = (from t in db.User_Rooms
                                          where t.UserID == userId && t.RoomID == _myRoomInfo.RoomID
                                          select t.CreateTime).FirstOrDefault();

                var chat = (from c in db.Chats
                            where c.RoomNo == _myRoomInfo.RoomID && c.CreateTime >= UserEnterTime
                            select c).ToList();

                foreach (var a in chat)
                {
                    // -------------------------------------
                    // 이부분이 새로운 메세지 그 부분
                    // -------------------------------------
                    var x = (from c in db.User_Chats
                            where c.RoomID == _myRoomInfo.RoomID && c.UserID == userId && c.ChatID == a.chatID
                            select c).FirstOrDefault();

                    if (x != null)
                    {
                        x.Check = true;
                    }

                        Chatlist.Add(new ChatMeta()
                    {
                        UserID = a.UserNo,
                        Content = a.Content,
                        CreateTime = a.CreateTime.ToString("yyyy/MM/dd HH:mm")
                    });
                }
                db.SaveChanges();
            }

            return Chatlist;
        }



        #endregion

        // 이 채팅방에 속한 유저들에게만 메세지 보내기

        public async void SaveChatLog(string userId, int roomId, string msg, DateTime createTime)
        {
            // 이부분에 테이블에 값 넣어주는거 추가
            // chat 테이블에 추가
            using (var db = new DBContext())
            {
                db.Add(new DB.Chat
                {
                    RoomNo = roomId,
                    UserNo = userId,
                    Content = msg,
                    CreateTime = createTime
                });
                db.SaveChanges();

                var chat = db.Chats!.ToList().LastOrDefault()!;

                // 이부분이 새로운 메세지 그 부분
                if(userId != "sys")
                {
                    foreach (var a in _myUserList)
                    {
                        var checkInfo = false;

                        if (a == userId)
                        {
                            checkInfo = true;
                        }

                        db.Add(new User_Chat
                        {
                            UserID = a,
                            RoomID = roomId,
                            ChatID = chat.chatID,
                            Check = checkInfo,
                            CreateTime = DateTime.Now
                        });
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        public void BroadcastToUsers(string userId, string msg, DateTime createTime)
        {
            lastChatTime = createTime;
            SaveChatLog(userId, _myRoomInfo.RoomID, msg, createTime);
            foreach (var user in _myUserList)
            {
                if (user != userId)
                {
                    JObject GuestMsg = new JObject(
                        new JProperty("msgType", "Chat_GuestInfo"),
                        new JProperty("ChatRoomId", _myRoomInfo.RoomID),
                        new JProperty("ChatRoomName", _myRoomInfo.RoomName),
                        new JProperty("UserName", userId),
                        new JProperty("Content", msg),
                        new JProperty("CreDate", createTime.ToString("yyyy/MM/dd HH:mm"))
                        );
                    string GuestSendMsg = JsonConvert.SerializeObject(GuestMsg);
                    ServerRuntime.Instance.SendMessage(user, GuestSendMsg);
                }
            }
        }
    }
}
