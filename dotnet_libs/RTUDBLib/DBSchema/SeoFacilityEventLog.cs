using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTUDBLib.DBSchema
{
    public partial class SeoFacilityEventLog
    {
        public decimal ID { get; set; }
        public decimal FacilityId { get; set; }
        public string LogLevel { get; set; }
        public DateTime LogDateTime { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
    }
}
