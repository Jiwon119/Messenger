using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoRtu
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Sn { get; set; }
        public string Ip { get; set; }
        public decimal Port { get; set; }
        public string Site { get; set; }
        public decimal Enable { get; set; }
    }
}
