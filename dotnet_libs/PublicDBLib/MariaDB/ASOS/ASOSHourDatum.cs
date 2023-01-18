using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.ASOS
{
    [Table("ASOSHourData")]
    [Index(nameof(stnId), nameof(tm), IsUnique = true)]
    public partial class ASOSHourDatum
    {
        public long Id { get; set; }

        public DateTime tm { get; set; }
        public string stnId { get; set; }
        public string stnNm { get; set; }
        public decimal? ta { get; set; } // 4 2
        public byte taQcflag { get; set; }
        public decimal? rn { get; set; } // 4 2
        public byte rnQcflag { get; set; }
        public decimal? ws { get; set; }
        public byte wsQcflag { get; set; }
        public short? wd { get; set; }
        public byte wdQcflag { get; set; }
        public decimal? hm { get; set; }
        public byte hmQcflag { get; set; }
        public decimal? pa { get; set; }
        public byte paQcflag { get; set; }
        public decimal? ps { get; set; }
        public byte psQcflag { get; set; }
        public decimal? ts { get; set; }
        public byte tsQcflag { get; set; }
        public decimal? ss { get; set; }
        public byte ssQcflag { get; set; }

        public decimal? pv { get; set; }
        public decimal? td { get; set; }
        public decimal? icsr { get; set; }
        public decimal? dsnw { get; set; }
        public decimal? hr3Fhsc { get; set; }
        public decimal? dc10Tca { get; set; }
        public decimal? dc10LmcsCa { get; set; }
        public short? clfmAbbrCd { get; set; }
        public short? lcsCh { get; set; }
        public short? vs { get; set; }
        public short? gndSttCd { get; set; }
        public short? dmstMtphNo { get; set; }
        public decimal? m005Te { get; set; }
        public decimal? m01Te { get; set; }
        public decimal? m02Te { get; set; }
        public decimal? m03Te { get; set; }
    }
}
