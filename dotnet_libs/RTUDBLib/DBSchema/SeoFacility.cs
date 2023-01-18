using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoFacility
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string DoorState { get; set; }
        public decimal? Type { get; set; }
        public string InsideWaterlevel { get; set; }
        public string OutsideWaterlevel { get; set; }
        public string WaterCourse { get; set; }
        public string FacType { get; set; }
        public string RtuList { get; set; }

        public string Address { get; set; }

        public string Cam_List { get; set; }

        public string Pos1 { get; set; }
        public string Pos2 { get; set; }
    }
}
