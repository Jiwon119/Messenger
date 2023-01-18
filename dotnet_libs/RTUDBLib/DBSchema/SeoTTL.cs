using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTUDBLib.DBSchema
{
    public partial class SeoTTL
    {
        public decimal Id { get; set; }

        public string TypeId { get; set; }

        public DateTime LastCheckTime { get; set; }
        public decimal LiveLimitSecond { get; set; }

    }
}
