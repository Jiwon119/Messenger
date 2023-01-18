using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.Reservior
{
    [Table("RsvDailyData")]
    [Index(nameof(FacCode), nameof(CheckDate), IsUnique = true)]
    public class RsvDailyDatum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Comment("저수지코드")]
        public string FacCode { get; set; }

        [Comment("저수지이름")]
        public string FacName { get; set; }

        [Comment("저수지위치")]
        public string Country { get; set; }

        [Comment("측정날짜")]
        public DateTime CheckDate { get; set; }

        [Comment("저수지수위")]
        [Precision(20, 2)]
        public decimal? WaterLevel { get; set; }

        [Comment("저수율")]
        [Precision(20, 2)]
        public decimal? Rate { get; set; }
    }
}
