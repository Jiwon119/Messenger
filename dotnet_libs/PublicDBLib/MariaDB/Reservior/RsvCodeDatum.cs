using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.Reservior
{
    [Table("RsvCodeList")]
    [Index(nameof(FacCode), IsUnique = true)]
    public class RsvCodeDatum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Comment("표준 코드")]
        public string FacCode { get; set; }

        [Comment("시설명")]
        public string FacName { get; set; }

        [Comment("소재지")]
        public string Location { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Comment("착공년도")]
        public DateTime YearOfStart { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Comment("준공년도")]
        public DateTime YearOfConstruction { get; set; }

        [Comment("종별")]
        public int? Classification { get; set; }

        [Comment("구분")]
        public string Division { get; set; }

        [Comment("유역면적")]
        [Precision(20, 2)]
        public decimal? WatershedArea { get; set; }

        [Comment("홍수면적")]
        [Precision(20, 2)]
        public decimal? FloodArea { get; set; }

        [Comment("만수면적")]
        [Precision(20, 2)]
        public decimal? FullArea { get; set; }

        [Comment("수혜면적")]
        [Precision(20, 2)]
        public decimal? BeneficiaryArea { get; set; }

        [Comment("한발빈도")]
        public int? OneStepFrequency { get; set; }

        [Comment("홍수빈도")]
        public int? FloodFrequency { get; set; }

        [Comment("제체형식")]
        public string BodyFormat { get; set; }

        [Comment("제체제적")]
        [Precision(20, 2)]
        public decimal? BodyVolume { get; set; }

        [Comment("제체길이")]
        [Precision(20, 2)]
        public decimal? BodyLength { get; set; }

        [Comment("제체높이")]
        [Precision(20, 2)]
        public decimal? BodyHeight { get; set; }

        [Comment("총저수량")]
        [Precision(20, 2)]
        public decimal? TotalStorage { get; set; }

        [Comment("유효저수량")]
        [Precision(20, 2)]
        public decimal? EffectiveStorage { get; set; }

        [Comment("사수량")]
        [Precision(20, 2)]
        public decimal? Gunnery { get; set; }

        [Comment("취수형식")]
        public string WaterIntake { get; set; }

        [Comment("홍수위")]
        [Precision(20, 2)]
        public decimal? FloodLevel { get; set; }

        [Comment("만수위")]
        [Precision(20, 2)]
        public decimal? WaterLevel { get; set; }

        [Comment("사수위")]
        [Precision(20, 2)]
        public decimal? Janitor { get; set; }
    }
}
