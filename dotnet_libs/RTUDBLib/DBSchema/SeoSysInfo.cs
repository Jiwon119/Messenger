using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTUDBLib.DBSchema
{
    public partial class SeoSysInfo
    {
        public decimal Id { get; set; }
        public decimal RtuId { get; set; }
        public DateTime Checkdatetime { get; set; }

        public decimal CpuCount { get; set; }
        public decimal CpuPersent { get; set; }
        public decimal MemPersent { get; set; }
        public decimal HDDPersent { get; set; }

        public string SysData { get; set; }
    }
}
