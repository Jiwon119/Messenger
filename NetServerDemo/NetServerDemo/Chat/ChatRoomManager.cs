using NetServerDemo.DB;
using NetServerDemo.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// 지시를 내리는.. 그런... 그런거...
// 개별 채팅방 정보는 chatRoom에서 가지고 오고...

namespace NetServerDemo.Chat
{
    internal class ChatRoomManager : Singleton<ChatRoomManager>
    {
        // 채팅방의 정보를 가지고 있음(모든 채팅방 X, 로그인(접속)한 유저의 채팅방 정보만 가지고 있도록)
        Dictionary<int, ChatRoom> _chatRooms = new Dictionary<int, ChatRoom>();

        // 유저가 로그인을 하면 채팅방의 정보를 받아 올 수 있도록
        public void OnUserLogin(string userId)
        {
            using (var ctx = new DBContext())
            {
                // 해당 유저가 속한 room id 리스트
                List<int> room_ids = new List<int>();
                room_ids = (from r in ctx.User_Rooms
                            where r.UserID == userId
                            select r.RoomID).ToList();

                // 해당 유저가 속한 room 리스트
                List<Room> roomList = new List<Room>();
                
                foreach (var id in room_ids)
                {
                    // room -> 채팅방 id, 이름, 생성날짜
                    var room = (from r in ctx.Rooms
                                where r.RoomID == id
                                select r).FirstOrDefault();

                    if (null != room)
                        roomList.Add(room);
                }

                foreach (var room in roomList)
                {
                    if (_chatRooms.ContainsKey(room.RoomID))
                        continue;

                    _chatRooms.Add(room.RoomID, new ChatRoom(room));
                }
            }
        }

        // 채팅방 목록 표시
        public void ShowRoomList(string userId)
        {
            // 유저 아이디가 없는경우 retrun
            if (string.IsNullOrEmpty(userId))
                return;

            // roomList에 처음에 저장했던 _chatRooms값을 넣어서 보내줌
            List<RoomMeta> roomList = new List<RoomMeta>();
            _chatRooms = _chatRooms.OrderByDescending(x => x.Value.lastChatTime).ToDictionary(x=>x.Key, x => x.Value);
            foreach (var room in _chatRooms.Values)
            {
                if (!room.IsValidUser(userId))
                    continue;

                roomList.Add(room.GetMyRoomMeta(userId));
            }

            JObject Msg = new JObject(
                new JProperty("msgType", "ChatRoomList"),
                new JProperty("RoomList", JArray.FromObject(roomList))
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            ServerRuntime.Instance.SendMessage(userId, SendMsg);

            #region 주석
            //if (string.IsNullOrEmpty(UserID))
            //    return;

            //// 해당 유저가 속해있는 채팅방 리스트
            //if (!_joinRoomLookup.TryGetValue(UserID, out List<int>? roomList))

            //    return;

            //foreach (var room in roomList)
            //{
            //    if (!_rooms.TryGetValue(room, out ChatRoom? r))
            //        continue;

            //    if (null == r)
            //        continue;

            //    r.SyncRoomInfo(UserID);
            //}
            #endregion
        }

        // 채팅방 생성 요청시( 채팅방 생성 후 포함 유저에게 알림 표시 )
        public void CreateRoom(string userId, string roomName, Array userList)
        {
            Room? room = null;

            using (var ctx = new DBContext())
            {
                room = new Room()
                {
                    RoomName = roomName,
                    CreateTime = DateTime.Now
                };

                ctx.Rooms!.Add(room);
                ctx.SaveChanges();

                room = ctx.Rooms!.ToList().LastOrDefault()!;

                ctx.Add(new User_Room
                {
                    UserID = userId,
                    RoomID = room.RoomID,
                    RoomLeader = true,
                    CreateTime = DateTime.Now
                    
                });

                foreach (var a in userList)
                {
                    ctx.Add(new User_Room
                    {
                        UserID = a.ToString(),
                        RoomID = room.RoomID,
                        RoomLeader = false,
                        CreateTime = DateTime.Now
                    });
                }

                ctx.SaveChanges();
            }

            if (null == room)
                return;

            ChatRoom r = new ChatRoom(room);
            _chatRooms.Add(room.RoomID, r);

            JObject HostMsg = new JObject(
                new JProperty("msgType", "CreateRoom"),
                new JProperty("RoomID", room.RoomID)
                );
            string HostSendMsg = JsonConvert.SerializeObject(HostMsg);
            ServerRuntime.Instance.SendMessage(userId, HostSendMsg);

            foreach (var u in userList)
            {
                JObject GuestMsg = new JObject(
                    new JProperty("msgType", "CreateRoom_GuestInfo"),
                    new JProperty("RoomID", room.RoomID)
                    );
                string GuestSendMsg = JsonConvert.SerializeObject(GuestMsg);
                ServerRuntime.Instance.SendMessage(u.ToString()!, GuestSendMsg);
            }
        }

        #region 채팅방 나가기
        public void OutRoom(string userId, int roomId)
        {
            _chatRooms[roomId].ChatRoomOutUser(userId);


            JObject Msg = new JObject(
                new JProperty("msgType", "success_ChatRoomOut")
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            ServerRuntime.Instance.SendMessage(userId, SendMsg);
        }

        public void ChatRoomOut(string userId, int roomId)
        {
            if (userId == _chatRooms[roomId].GetMyRoomMeta(userId).RoomLeader)
            {
                if (_chatRooms[roomId].GetMyRoomMeta(userId).RoomUser.Count() == 1)
                {
                    OutRoom(userId, roomId);
                }
                else
                {
                    JObject ChangeLeaderMsg = new JObject(
                        new JProperty("msgType", "error_ChangeRoomLeader")
                        );
                    string ChangeLeaderSendMsg = JsonConvert.SerializeObject(ChangeLeaderMsg);
                    ServerRuntime.Instance.SendMessage(userId, ChangeLeaderSendMsg);
                }
            }
            else
            {
                OutRoom(userId, roomId);
            }
        }
        #endregion

        #region 개별채팅방 표시

        // 메세지를 보냈는데 상대 유저가 채팅방에 존재하는 경우
        // User_Chat check => true
        public void UserInChatRoom(string userId, int roomId)
        {
            using(var db = new DBContext())
            {
                var userInRoom = (from check in db.User_Chats
                                  where check.RoomID == roomId && check.UserID == userId && check.Check == false
                                  select check).ToList();
                foreach(var c in userInRoom)
                {
                    c.Check = true;
                }
                db.SaveChanges();
            }
        }

        // 채팅방 클릭했을때 해당 채팅방 id 넘겨주기 위함
        public void ChatRoomClick(string userId, int roomId)
        {
            JObject Msg = new JObject(
                new JProperty("msgType", "RoomEnter"),
                new JProperty("RoomNum", roomId.ToString())
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            ServerRuntime.Instance.SendMessage(userId, SendMsg);
        }

        // 사용자 정보, 채팅방 이름 등 전송
        public void ReqChatRoomInfo(string userId, int roomId)
        {
            RoomMeta room = _chatRooms[roomId].GetMyRoomMeta(userId);
            JObject GuestMsg = new JObject(
                new JProperty("msgType", "ChatRoomInfo"),
                new JProperty("UserName", userId),
                new JProperty("RoomName", room.RoomName),
                new JProperty("RoomLeader", room.RoomLeader),
                new JProperty("RoomUser", JArray.FromObject(room.RoomUser))
                );
            string GuestSendMsg = JsonConvert.SerializeObject(GuestMsg);
            ServerRuntime.Instance.SendMessage(userId, GuestSendMsg);
        }

        // 대화 목록 표시
        public void ReqChatInfo(string userId, int roomId)
        {
            JObject Msg = new JObject(new JProperty("msgType", "ChatList"));
            List<ChatMeta> ChatList = new List<ChatMeta>();
            ChatList = _chatRooms[roomId].GetMyChatMeta(userId);
            Msg.Add("ChatList", JArray.FromObject(ChatList));

            string SendMsg = JsonConvert.SerializeObject(Msg);
            ServerRuntime.Instance.SendMessage(userId, SendMsg);
        }

        #endregion

        #region 채팅방 수정

        // modify 화면에 기본 정보 출력
        public void ReqChatRoomModifyUserList(string userId, int roomId)
        {
            List<string> allUsers = new List<string>();
            List<string> InUser = new List<string>();
            List<string> OutUser = new List<string>();

            using (var db = new DBContext())
            {
                allUsers = (from u in db.Users
                            select u.UserID).ToList();
            }
            allUsers.Remove("sys");

            foreach (var a in allUsers)
            {
                if(a != userId)
                {
                    if (_chatRooms[roomId].IsValidUser(a) || a.Equals(userId))
                    {
                        InUser.Add(a);
                    }
                    else
                    {
                        OutUser.Add(a);
                    }
                }
            }

            JObject Msg = new JObject(
                new JProperty("msgType", "ChatRoomModifyUserList"),
                new JProperty("RoomName", _chatRooms[roomId].GetMyRoomMeta(userId).RoomName),
                new JProperty("InUser", JArray.FromObject(InUser)),
                new JProperty("OutUser", JArray.FromObject(OutUser))
            );

            string SendMsg = JsonConvert.SerializeObject(Msg);
            ServerRuntime.Instance.SendMessage(userId, SendMsg);
        }

        // 수정한 채팅방 정보 DB에 입력
        public void ModifyChatRoom(string userId, int roomId, string editedRoomName, Array removeUser, Array addUser, string changeLeaderName)
        {
            _chatRooms[roomId].SetRoomName(editedRoomName);
            _chatRooms[roomId].SetUserList(removeUser, addUser);
            _chatRooms[roomId].SetRoomLeader(changeLeaderName);

            foreach(var a in addUser)
            {
                JObject invitedUserMsg = new JObject(
                    new JProperty("msgType", "CreateRoom_GuestInfo"),
                    new JProperty("RoomID", roomId)
                );
                string invitedUserSendMsg = JsonConvert.SerializeObject(invitedUserMsg);
                ServerRuntime.Instance.SendMessage(a.ToString()!, invitedUserSendMsg);
            }

            JObject Msg = new JObject(
                new JProperty("msgType", "ModifySuccess")
            );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            ServerRuntime.Instance.SendMessage(userId, SendMsg);
        }

        #endregion

        // 메세지 보내기
        public void SendToMessage(string userId, int roomId, string msg, DateTime createTime)
        {

            if (!_chatRooms.TryGetValue(roomId, out ChatRoom? room))
                return;

            if (room == null)
                return;

            if (!room.IsValidUser(userId))
                return;

            room.BroadcastToUsers(userId, msg, createTime);
        }
    }
}
