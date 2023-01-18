
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicDBLib.MariaDB.ASOS
{
    [Table("ASOSCodeList")]
    [Index(nameof(WPCode), IsUnique = true)]
    public partial class ASOSCodeDatum
    {
        public int Id { get; set; }

        public string WPCode { get; set; }
        public string WPName { get; set; }
        public string WPMgrName { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }

        [NotMapped]
        public Point location { get; set; }
    }
}
