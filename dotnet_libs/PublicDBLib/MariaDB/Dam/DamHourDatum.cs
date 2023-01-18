using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.Dam
{
    [Table("DamHourData")]
    [Index(nameof(DamCode), nameof(obsrdt), IsUnique = true)]
    public partial class DamHourDatum
    {
        public long Id { get; set; }

        public string DamCode { get; set; }
        public DateTime obsrdt { get; set; }
        public decimal? lowlevel { get; set; }
        public decimal? rf { get; set; }
        public decimal? inflowqy { get; set; }
        public decimal? totdcwtrqy { get; set; }
        public decimal? rsvwtqy { get; set; }
        public decimal? rsvwtrt { get; set; }
    }
}
