using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoSensorDatum
    {
        public decimal Id { get; set; }
        public decimal RtuId { get; set; }
        public string SensorId { get; set; }
        public DateTime Obsdatetime { get; set; }
        public decimal Value { get; set; }

        public decimal? ProcValue { get; set; }
    }
}
