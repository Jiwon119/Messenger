using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoUnit
    {
        public decimal Id { get; set; }
        public decimal RtuId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ValueName { get; set; }
        public decimal AnnotationMin { get; set; }
        public decimal AnnotationMax { get; set; }
        public string Context { get; set; }
        public decimal ValueMin { get; set; }
        public decimal ValueMax { get; set; }
        public string StateType { get; set; }
        public string TypeCode { get; set; }
        public decimal? WaterLaneId { get; set; }
        public decimal? GateOrder { get; set; }
        public decimal IsNewUnit { get; set; }
    }
}
