using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoDiscoveredRtu
    {
        public decimal Id { get; set; }
        public decimal State { get; set; }
        public string Ip { get; set; }
        public decimal Port { get; set; }
    }
}
