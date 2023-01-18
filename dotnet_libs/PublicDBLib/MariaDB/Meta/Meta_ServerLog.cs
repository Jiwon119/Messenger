using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.Meta
{
    [Table("ServerLogList")]
    public partial class Meta_ServerLog
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
    }
}
