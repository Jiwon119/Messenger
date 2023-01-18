using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoCctv
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Sn { get; set; }
        public string ConString { get; set; }
        public string Site { get; set; }
    }
}
