using CommonLib.Loggers;
using CommonLib.Network;
using CommonLib.Runtime;
using NetServerDemo.Chat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetServerDemo
{
    class ServerRuntime : RuntimeFramework
    {
        private static ServerRuntime? instance = null;
        public static ServerRuntime Instance => instance ?? (instance = new ServerRuntime());

        private SimpleNetServer? _server = null;

        protected override bool OnInitialized()
        {
            if (null != _server)
                return false;

            _server = NetServer.MakeSimpleServer("127.0.0.1", 6767) as SimpleNetServer;
            if (null == _server)
                return false;

            _server.OnOpen += OnOpen_Recv;
            _server.OnClose += OnClose_Recv;
            _server.OnMessage += OnMessage_Recv;
            _server.OnError += OnError_Recv;

            _server.StartServer();

            return true;
        }

        public void SendMessage(string userName, string msg)
        {
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            byte[] utf8Bytes = utf8.GetBytes(msg);
            string s64 = Convert.ToBase64String(utf8Bytes);

            if (_idLookup.ContainsKey(userName))
            {
                var connId = _idLookup[userName];
                _server!.SendMessage(connId, s64);
            }
        }

        public void SendMessage(ConnectionID id, string msg)
        {
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            byte[] utf8Bytes = utf8.GetBytes(msg);
            string s64 = Convert.ToBase64String(utf8Bytes);

            _server!.SendMessage(id, s64);
        }

        // 유저 리스트
        private Dictionary<ConnectionID, string> _users = new Dictionary<ConnectionID, string>();

        // 유저 이름으로 검색하기 위한 dictionary
        private Dictionary<string, ConnectionID> _idLookup = new Dictionary<string, ConnectionID>();

        // 현재 접속중인 유저 리스트
        private Dictionary<ConnectionID, string> _connectUser = new Dictionary<ConnectionID, string>();

        #region network event
        private void OnOpen_Recv(ConnectionID id, string msg)
        {
            NLogger.Get.Log(NLog.LogLevel.Info, msg);
            //_users.Add(id);
        }
        private void OnClose_Recv(ConnectionID id, string msg)
        {
            NLogger.Get.Log(NLog.LogLevel.Info, msg);
            _connectUser.Remove(id);

            if (_users.TryGetValue(id, out string? strId))
            {
                if (!string.IsNullOrEmpty(strId))
                    _idLookup.Remove(strId);
            }

            _users.Remove(id);
        }
        private void OnError_Recv(ConnectionID id, string msg)
        {
            NLogger.Get.Log(NLog.LogLevel.Info, msg);
        }
        private void OnMessage_Recv(ConnectionID id, string msg)
        {

            NLogger.Get.Log(NLog.LogLevel.Info, msg);

            string messageType;

            JObject? jmsg = null;
            try
            {
                var jobj = JObject.Parse(msg);
                messageType = (string)jobj["msgType"]!;
                jmsg = jobj!;
            }
            catch (Exception e)
            {
                //
                NLogger.Get.Error(e.Message);
                return;
            }

            if (string.IsNullOrWhiteSpace(messageType))
            {
                // message is empty;
                return;
            }
            _users.TryGetValue(id, out string? reqUserID);

            switch (messageType)
            {
                case "ProfilSave":
                    UserProfilSave(id, jmsg);
                    break;
                case "imgRead": // UserImgList
                    UserImgList(id, jmsg);
                    break;
                case "UserImg":
                    UserImg(id, jmsg);
                    break;
                case "Login":
                    Login(id, jmsg);
                    break;
                case "SignUp":
                    SignUp(id, jmsg);
                    break;
                case "Logout":
                    Logout(id);
                    break;
                case "ReqUserList":
                    RequestUser(id);
                    break;
                case "ReqName":
                    RequestMyName(id);
                    break;
                case "RefUserInfo":
                    ReqUserInfo(id, jmsg);
                    break;
                case "Identification":
                    Identification(id, jmsg["password"]!.ToString());
                    break;
                case "ModifyUserInfo":
                    ModifyUserInfo(id, jmsg);
                    break;
                case "CreateChatRoom":
                    var ChatRoomName = jmsg["chatRoomName"]!.ToString();
                    var ChatRoomUserList = jmsg["chatRoomUser"]!.ToArray();
                    ChatRoomManager.Get.CreateRoom(reqUserID!, ChatRoomName!, ChatRoomUserList);
                    break;
                case "showRoomList":
                    ChatRoomManager.Get.ShowRoomList(reqUserID!);
                    break;
                case "ChatRoomClick":
                    ChatRoomManager.Get.ChatRoomClick(reqUserID!, (int)jmsg["chatInfo"]!);
                    break;
                case "ChatRoomSendMsg":
                    var RoomID = (int)jmsg["RoomID"]!;
                    var Content = jmsg["Content"]!.ToString();
                    DateTime createTime = DateTime.Now;
                    ChatRoomManager.Get.SendToMessage(reqUserID!, RoomID, Content, createTime);
                    break;
                case "UserInChatRoom":
                    var roomID = (int)jmsg["RoomID"]!;
                    ChatRoomManager.Get.UserInChatRoom(reqUserID!, roomID);
                    break;
                case "ReqChatInfo":
                    ChatRoomManager.Get.ReqChatInfo(reqUserID!, (int)jmsg["RoomID"]!);
                    break;
                case "ReqChatRoomInfo":
                    ChatRoomManager.Get.ReqChatRoomInfo(reqUserID!, (int)jmsg["RoomID"]!);
                    break;
                case "ReqChatRoomModifyUserList":
                    ChatRoomManager.Get.ReqChatRoomModifyUserList(reqUserID!, (int)jmsg["RoomID"]!);
                    break;
                case "ModifyChatRoom":
                    ChatRoomManager.Get.ModifyChatRoom(reqUserID!, (int)jmsg["RoomID"]!, jmsg["ChatRoomName"]!.ToString()
                        , jmsg["RemoveUserSelect"]!.ToArray(), jmsg["AddUserSelect"]!.ToArray(), jmsg["LeaderChange"]!.ToString());
                    break;
                case "ChatRoomOut":
                    ChatRoomManager.Get.ChatRoomOut(reqUserID!, (int)jmsg["RoomID"]!);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region msgHandling
        private void Login(ConnectionID id, JObject json)
        {
            using (var db = new DBContext())
            {
                string LoginID = json["LoginID"]!.ToString();
                string LoginPW = json["LoginPW"]!.ToString();
                var UserDBList = db.Users?.Find(LoginID);

                if (UserDBList != null)
                {
                    // 아이디, 패스워드 확인
                    if (UserDBList.UserPW == LoginPW)
                    {
                        // 현재 접속중인 유저 목록에 있는지 확인
                        if (!_connectUser.ContainsValue(LoginID))
                        {
                            _connectUser.Add(id, LoginID);
                            var id_ = LoginID;
                            if (!_users.ContainsKey(id))
                            {
                                _users.Add(id, id_);
                            }

                            _idLookup.Add(id_, id);
                            //ChatRoomManager.Get.SyncChatRoomInfo(id_);
                            ChatRoomManager.Get.OnUserLogin(LoginID);

                            // 로그인 성공 패킷 보냄
                            JObject Msg = new JObject(new JProperty("msgType", "Login success"));
                            string SendMsg = JsonConvert.SerializeObject(Msg);
                            SendMessage(id, SendMsg);
                        }
                        else
                        {
                            // 이미 접속중인 유저
                            JObject Msg = new JObject(new JProperty("msgType", "connected user"));
                            string SendMsg = JsonConvert.SerializeObject(Msg);
                            SendMessage(id, SendMsg);
                        }
                        
                    }
                    else
                    {
                        // 패스워드 틀림
                        JObject Msg = new JObject(new JProperty("msgType", "Wrong password"));
                        string SendMsg = JsonConvert.SerializeObject(Msg);
                        SendMessage(id, SendMsg);
                    }
                }
                else
                {
                    // 없는 아이디
                    JObject Msg = new JObject(new JProperty("msgType", "Login fail"));
                    string SendMsg = JsonConvert.SerializeObject(Msg);
                    SendMessage(id, SendMsg);
                }
            }
        }
        private void SignUp(ConnectionID id, JObject json)
        {
            using (var db = new DBContext())
            {
                var UserDBList = db.Users?.Find(json["NewUserID"]?.ToString());
                if (UserDBList == null)
                {
                    db.Add(new DB.User
                    {
                        UserID = json["NewUserID"]?.ToString(),
                        UserPW = json["NewUserPW"]?.ToString(),
                        UserName = json["NewUserName"]?.ToString(),
                        UserPhon = json["NewUserPhon"]?.ToString()
                    });
                    db.Add(new DB.User_Profile
                    {
                        UserId = json["NewUserID"]?.ToString(),
                        // 기본 프로필 사진
                        Img = "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"
                    });

                    db.SaveChanges();
                    SendMessage(id, "Sign up success");
                }
                else
                {
                    SendMessage(id, "The username that already exists.");
                }
            }
        }
        private void Identification(ConnectionID id, string password)
        {
            using(var db = new DBContext())
            {
                DB.User user = db.Users?.Find(_users[id])!;
                if ( password == user.UserPW)
                {
                    JObject Msg = new JObject(
                        new JProperty("msgType", "Identification success"),
                        new JProperty("userName", user.UserName),
                        new JProperty("userPhon", user.UserPhon)
                        );
                    string SendMsg = JsonConvert.SerializeObject(Msg);
                    SendMessage(id, SendMsg);
                }
                else
                {
                    JObject Msg = new JObject(new JProperty("msgType", "Identification fail"));
                    string SendMsg = JsonConvert.SerializeObject(Msg);
                    SendMessage(id, SendMsg);
                }

            }
        }
        private void ModifyUserInfo(ConnectionID id, JObject json)
        {
            using(var db = new DBContext())
            {
                DB.User user = db.Users!.Find(_users[id])!;
                user.UserName = json["userName"]!.ToString();
                user.UserPhon = json["usrPhon"]!.ToString();

                if(null != json["userPw"])
                {
                    user.UserPW = json["userPw"]!.ToString();
                }

                db.SaveChanges();
            }
            JObject Msg = new JObject(
                new JProperty("msgType", "modify completed")
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            SendMessage(id, SendMsg);
        }
        private void Logout(ConnectionID id)
        {
            _connectUser.Remove(id);
            _idLookup.Remove(_users[id]);
            _users.Remove(id);

            JObject Msg = new JObject(
                new JProperty("msgType", "Logout Success")
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            SendMessage(id, SendMsg);
        }
        private void RequestUser(ConnectionID id)
        {
            using (var db = new DBContext())
            {
                var UserDBList = db.Users?.ToList();
                

                if (UserDBList != null)
                {
                    List<Dictionary<string, string>> UserList = new List<Dictionary<string, string>>();
                    JObject Msg = new JObject(new JProperty("msgType", "UserListReq"));
                    foreach (var item in UserDBList)
                    {
                        if (item.UserID != _users[id] && item.UserID != "sys")
                        {
                            Dictionary<string, string> dict = new Dictionary<string, string>();
                            dict.Add("UserID", item.UserID!);
                            dict.Add("UserName", item.UserName!);
                            if (_connectUser.ContainsValue(item.UserID!))
                            {
                                dict.Add("ConnectInfo", "true");
                            }
                            else
                            {
                                dict.Add("ConnectInfo", "false");
                            }
                            var UserImg = db.User_Profiles?.Find(item.UserID?.ToString())?.Img;
                            if(UserImg == null)
                            {
                                dict.Add("UserImg", "");
                            }
                            else
                            {
                                dict.Add("UserImg", UserImg.ToString());
                            }
                            
                            UserList.Add(dict);
                        }
                    }
                    Msg.Add("UserList", JArray.FromObject(UserList));
                    string SendMsg = JsonConvert.SerializeObject(Msg);
                    SendMessage(id, SendMsg);
                }
                else
                {
                    SendMessage(id, "fail");
                }
            }
        }
        private void RequestMyName(ConnectionID id)
        {
            JObject Msg = new JObject(
                new JProperty("msgType", "NameReq"),
                new JProperty("MyName", _users[id])
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            SendMessage(id, SendMsg);
        }
        private void ReqUserInfo(ConnectionID id, JObject json)
        {
            using(var db = new DBContext())
            {
                Console.WriteLine(json["userId"]!.ToString());
                DB.User user = db.Users!.Find(json["userId"]!.ToString())!;

                JObject Msg = new JObject(
                new JProperty("msgType", "UserInfo"),
                new JProperty("userName", user.UserName!),
                new JProperty("userPhon", user.UserPhon!)
                );
                string SendMsg = JsonConvert.SerializeObject(Msg);
                SendMessage(id, SendMsg);
            }
        }
        private void UserProfilSave(ConnectionID id, JObject json)
        {
            using (var db = new DBContext())
            {
                // 이미 프로필 사진을 입력한 상황이면 사진 수정
                // 아닌경우 새로 등록
                if (db.User_Profiles!.Find(_users[id]) != null)
                {
                    db.User_Profiles.Find(_users[id])!.Img = json["imgSource"]!.ToString();
                    db.SaveChanges();
                }
                else
                {
                    db.Add(new DB.User_Profile
                    {
                        UserId = _users[id].ToString(),
                        Img = json["imgSource"]!.ToString()
                    });
                    db.SaveChanges();
                }
                JObject Msg = new JObject(
                    new JProperty("msgType", "MyImg")
                    );
                string SendMsg = JsonConvert.SerializeObject(Msg);
                SendMessage(id, SendMsg);
            }
        }
        private void UserImgList(ConnectionID id, JObject json)
        {
            string userimg;
            using (var db = new DBContext())
            {
                if (db.User_Profiles!.Find(_users[id]) != null)
                {
                    userimg = db.User_Profiles.Find(_users[id])?.Img!;
                }
                else
                {
                    userimg = "";
                }

            }
            JObject Msg = new JObject(
                new JProperty("msgType", "UserImg"),
                new JProperty("userimg", userimg)
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            SendMessage(id, SendMsg);
        }
        private void UserImg(ConnectionID id, JObject json)
        {
            string userimg;
            using (var db = new DBContext())
            {
                if (db.User_Profiles!.Find(_users[id]) != null)
                {
                    userimg = db.User_Profiles.Find(_users[id])?.Img!;
                }
                else
                {
                    userimg = "";
                }

            }
            JObject Msg = new JObject(
                new JProperty("msgType", "UserImg"),
                new JProperty("userimg", userimg)
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            SendMessage(id, SendMsg);
        }
        #endregion


        protected override void OnTerminated()
        {
            if (null != _server)
            {
                _server.TerminateServer();
                _server = null;
            }
        }

        protected override void OnTickEvent(long elapsedTime)
        {
            if (null != _server)
            {
                _server.UpdateServer();
            }
        }
    }
}