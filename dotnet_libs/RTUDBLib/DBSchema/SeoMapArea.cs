using System;
using System.Collections.Generic;

#nullable disable

namespace RTUDBLib.DBSchema
{
    public partial class SeoMapArea
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Pos1 { get; set; }
        public string Pos2 { get; set; }
        public string Type { get; set; }
        public string WaterLevel { get; set; }
        public string WaterLevelRate { get; set; }
        public string ConnectState { get; set; }
        public string IsWork { get; set; }
        public decimal SceneNum { get; set; }
        public decimal facType { get; set; }
    }
}
