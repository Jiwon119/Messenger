using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PublicDBLib.MariaDB.Dam
{
    [Table("DamCodeList")]
    [Index(nameof(DamCode), IsUnique = true)]
    public partial class DamCodeDatum
    {
        public int Id { get; set; }

        public string DamCode { get; set; }
        public string DamName { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }

        [NotMapped]
        public Point location { get; set; }
    }
}
