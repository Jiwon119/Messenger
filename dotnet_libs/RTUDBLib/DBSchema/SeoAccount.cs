using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoAccount
    {
        public string PermissionList { get; set; }
        public string Pw { get; set; }
        public string Id { get; set; }
        public decimal UniqueId { get; set; }
        public string RegDate { get; set; }
        public string PwSalt { get; set; }
        public decimal IsTempPw { get; set; }
        public decimal FailedCount { get; set; }
    }
}
