using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoCommand
    {
        public decimal Id { get; set; }
        public string Type { get; set; }
        public string Context { get; set; }
        public string State { get; set; }
        public string Rsp { get; set; }
    }
}
