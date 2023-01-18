using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServerDemo.DB
{
    public class Chat
    {
        public int? chatID { get; set; }
        public int? RoomNo { get; set; }
        public string? UserNo { get; set; }
        public string? Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
