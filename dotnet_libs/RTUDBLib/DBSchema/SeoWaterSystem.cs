using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTUDBLib.DBSchema
{
    internal class SeoWaterSystem
    {
        public decimal Id { get; set; }
        public decimal? SensorID { get; set; }
        public decimal? WaterLaneID { get; set; }
        public decimal? FacID { get; set; }

        public decimal? Order{ get; set; }
    }
}
