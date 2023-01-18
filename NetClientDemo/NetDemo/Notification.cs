using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDemo
{
    internal class Notification
    {
        /// <summary>
        /// 채팅 알림
        /// </summary>
        /// <param name="roomName">채팅방 이름</param>
        /// <param name="userInfo">유저 이름</param>
        /// <param name="chatContent">채팅 내용</param>
        /// <param name="time">시간</param>
        public void NoticeChat(JObject json)
        {
            var chatUserInfo = json["UserName"].ToString();
            var chatContentInfo = json["Content"].ToString();
            var chatRoomName = json["ChatRoomName"].ToString();
            var chatTime = json["CreDate"].ToString();

            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText($"{chatRoomName}")
                .AddText($"{chatUserInfo} : {chatContentInfo}\n\n{chatTime}")
                .Show();
        }
    }
}
