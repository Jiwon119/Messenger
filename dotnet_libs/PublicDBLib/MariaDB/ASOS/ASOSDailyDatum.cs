using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.ASOS
{
    [Table("ASOSDailyData")]
    [Index(nameof(stnId), nameof(tm), IsUnique = true)]
    public partial class ASOSDailyDatum
    {
        public long Id { get; set; }

        public DateTime tm { get; set; }
        public string stnId { get; set; }
        public string stnNm { get; set; }
        public decimal? avgTa { get; set; }
        public decimal? minTa { get; set; }
        public string minTaHrmt { get; set; }
        public decimal? maxTa { get; set; }
        public string maxTaHrmt { get; set; }
        public decimal? sumRnDur { get; set; }
        public decimal? mi10MaxRn { get; set; }
        public string mi10MaxRnHrmt { get; set; }
        public decimal? hr1MaxRn { get; set; }
        public string hr1MaxRnHrmt { get; set; }
        public decimal? sumRn { get; set; }
        public decimal? maxInsWs { get; set; }
        public short? maxInsWsWd { get; set; }
        public string maxInsWsHrmt { get; set; }
        public decimal? maxWs { get; set; }
        public short? maxWsWd { get; set; }
        public string maxWsHrmt { get; set; }
        public decimal? avgWs { get; set; }
        public short? hr24SumRws { get; set; }
        public decimal? maxWd { get; set; }
        public decimal? avgTd { get; set; }
        public decimal? minRhm { get; set; }
        public string minRhmHrmt { get; set; }
        public decimal? avgRhm { get; set; }
        public decimal? avgPv { get; set; }
        public decimal? avgPa { get; set; }
        public decimal? maxPs { get; set; }
        public string maxPsHrmt { get; set; }
        public decimal? minPs { get; set; }
        public string minPsHrmt { get; set; }
        public decimal? avgPs { get; set; }
        public decimal? ssDur { get; set; }
        public decimal? sumSsHr { get; set; }
        public string hr1MaxIcsrHrmt { get; set; }
        public decimal? hr1MaxIcsr { get; set; }
        public decimal? sumGsr { get; set; }
        public decimal? ddMefs { get; set; }
        public string ddMefsHrmt { get; set; }
        public decimal? ddMes { get; set; }
        public string ddMesHrmt { get; set; }
        public decimal? sumDpthFhsc { get; set; }
        public decimal? avgTca { get; set; }
        public decimal? avgLmac { get; set; }
        public decimal? avgTs { get; set; }
        public decimal? minTg { get; set; }
        public decimal? avgCm5Te { get; set; }
        public decimal? avgCm10Te { get; set; }
        public decimal? avgCm20Te { get; set; }
        public decimal? avgCm30Te { get; set; }
        public decimal? avgM05Te { get; set; }
        public decimal? avgM10Te { get; set; }
        public decimal? avgM15Te { get; set; }
        public decimal? avgM30Te { get; set; }
        public decimal? avgM50Te { get; set; }
        public decimal? sumLrgEv { get; set; }
        public decimal? sumSmlEv { get; set; }
        public decimal? n99Rn { get; set; }
        public string iscs { get; set; }
        public decimal? sumFogDur { get; set; }
        public string dt { get; set; }
    }
}
