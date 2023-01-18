using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoUsageLog
    {
        public decimal Id { get; set; }
        public decimal ActionCode { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? Value { get; set; }
        public string Target { get; set; }
        public string Actor { get; set; }
    }
}
