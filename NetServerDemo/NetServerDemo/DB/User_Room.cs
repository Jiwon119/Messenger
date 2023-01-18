using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServerDemo.DB
{
    internal class User_Room
    {
        public string? UserID { get; set; }
        public int RoomID { get; set; }
        public bool RoomLeader { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
