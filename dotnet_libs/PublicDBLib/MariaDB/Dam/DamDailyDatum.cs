using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.Dam
{
    [Table("DamDailyData")]
    [Index(nameof(DamCode), nameof(obsryymtde), IsUnique = true)]
    public partial class DamDailyDatum
    {
        public long Id { get; set; }

        public string DamCode { get; set; }
        public DateTime obsryymtde { get; set; }
        public decimal? lowlevel { get; set; }
        public decimal? prcptqy { get; set; }
        public decimal? inflowqy { get; set; }
        public decimal? totdcwtrqy { get; set; }
        public decimal? rsvwtqy { get; set; }
        public decimal? rsvwtrt { get; set; }
        public string dt { get; set; }
    }
}
