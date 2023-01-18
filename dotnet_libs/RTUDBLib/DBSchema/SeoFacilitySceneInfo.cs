using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTUDBLib.DBSchema
{
    public partial class SeoFacilitySceneInfo
    {
        public decimal Id { get; set; }

        public decimal FacilityId { get; set; }
        public decimal RtuId { get; set; }
        public string UnitId { get; set; }
        public decimal SceneNum { get; set; }
        public decimal RenderOrder { get; set; }
    }
}
