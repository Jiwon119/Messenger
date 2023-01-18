using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicDBLib.MariaDB.Join
{
    [Table("JoinRsvDailyData")]
    [Index(nameof(FacCode), nameof(CheckDate), IsUnique = true)]
    public class JoinRsvDailyDatum
    {
        [Key]
        public int Id { get; set; }

        // Reservoir
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

        // ASOS
        [Comment("종관기상관측 지점번호")]
        [MaxLength(20)]
        public string stnId { get; set; }

        [Comment("종관기상관측 지점명")]
        [MaxLength(20)]
        public string stnNm { get; set; }

        [Comment("평균기온(c)")]
        [Precision(6, 2)]
        public decimal? avgTa { get; set; }

        [Comment("최저기온(c)")]
        [Precision(6, 2)]
        public decimal? minTa { get; set; }

        [Comment("최저기온시간")]
        [MaxLength(5)]
        public string minTaHrmt { get; set; }

        [Comment("최고기온(c)")]
        [Precision(6, 2)]
        public decimal? maxTa { get; set; }

        [Comment("최고기온시간")]
        [MaxLength(5)]
        public string maxTaHrmt { get; set; }

        [Comment("강수계속시간")]
        [Precision(6, 2)]
        public decimal? sumRnDur { get; set; }

        [Comment("10분 최다강수량(mm)")]
        [Precision(6, 2)]
        public decimal? mi10MaxRn { get; set; }

        [Comment("10분 최다강수량 시각(mm)")]
        [Precision(6, 2)]
        public string mi10MaxRnHrmt { get; set; }

        [Comment("1시간 최다강수량(mm)")]
        [Precision(6, 2)]
        public decimal? hr1MaxRn { get; set; }

        [Comment("1시간 최다강수량 시각")]
        [MaxLength(5)]
        public string hr1MaxRnHrmt { get; set; }

        [Comment("일강수량(mm)")]
        [Precision(6, 2)]
        public decimal? sumRn { get; set; }

        [Comment("최대 순간풍속(m/s)")]
        [Precision(6, 2)]
        public decimal? maxInsWs { get; set; }

        [Comment("최대 순간풍속 풍향(16방위)")]
        [Column(TypeName = "SMALLINT(4)")]
        public short? maxInsWsWd { get; set; }

        [Comment("최대 순간풍속 시각")]
        [MaxLength(5)]
        public string maxInsWsHrmt { get; set; }

        [Comment("최대 풍속(m/s)")]
        [Precision(6, 2)]
        public decimal? maxWs { get; set; }

        [Comment("최대 충속 풍향(16방위)")]
        [Column(TypeName = "SMALLINT(4)")]
        public short? maxWsWd { get; set; }

        [Comment("최대 풍속 시각")]
        [MaxLength(5)]
        public string maxWsHrmt { get; set; }

        [Comment("평균 풍속(m/s)")]
        [Precision(6, 2)]
        public decimal? avgWs { get; set; }

        [Comment("풍정합(100m)")]
        [Column(TypeName = "SMALLINT(4)")]
        public short? hr24SumRws { get; set; }

        [Comment("최다 풍향(16방위)")]
        [Column(TypeName = "SMALLINT(4)")]
        public decimal? maxWd { get; set; }

        [Comment("평균 이슬점 온도(c)")]
        [Precision(6, 2)]
        public decimal? avgTd { get; set; }

        [Comment("최대 상대습도(%)")]
        [Precision(6, 2)]
        public decimal? minRhm { get; set; }

        [Comment("평균 상대습도 시각")]
        [MaxLength(5)]
        public string minRhmHrmt { get; set; }

        [Comment("평균 상대습도(%)")]
        [Precision(6, 2)]
        public decimal? avgRhm { get; set; }

        [Comment("평균 중기압(hPa)")]
        [Precision(6, 2)]
        public decimal? avgPv { get; set; }

        [Comment("평균 현지기압(hPa)")]
        [Precision(6, 2)]
        public decimal? avgPa { get; set; }

        [Comment("최고 해면기압")]
        [Precision(6, 2)]
        public decimal? maxPs { get; set; }

        [Comment("최고 해면기압 시각")]
        [MaxLength(5)]
        public string maxPsHrmt { get; set; }

        [Comment("최저 해면기압")]
        [Precision(6, 2)]
        public decimal? minPs { get; set; }

        [Comment("최저 해면기압 시각")]
        [MaxLength(5)]
        public string minPsHrmt { get; set; }

        [Comment("평균 해면기압(hPa)")]
        [Precision(6, 2)]
        public decimal? avgPs { get; set; }

        [Comment("가조시간(hr)")]
        [Precision(6, 2)]
        public decimal? ssDur { get; set; }

        [Comment("합계 일조 시간(hr)")]
        [Precision(6, 2)]
        public decimal? sumSsHr { get; set; }

        [Comment("1시간 최다 일사시각(MJ/m2)")]
        [MaxLength(5)]
        public string hr1MaxIcsrHrmt { get; set; }

        [Comment("1시간 최대 일사량(MJ/m2)")]
        [Precision(6, 2)]
        public decimal? hr1MaxIcsr { get; set; }

        [Comment("합계 일사량(MJ/m2)")]
        [Precision(6, 2)]
        public decimal? sumGsr { get; set; }

        [Comment("일 최심신적설(cm)")]
        [Precision(6, 2)]
        public decimal? ddMefs { get; set; }

        [Comment("일 최심신적설 시각")]
        [MaxLength(5)]
        public string ddMefsHrmt { get; set; }

        [Comment("일 최심적설(cm)")]
        [Precision(6, 2)]
        public decimal? ddMes { get; set; }

        [Comment("일 최심적설 시각")]
        [MaxLength(5)]
        public string ddMesHrmt { get; set; }

        [Comment("합계 3시간 신적설(cm)")]
        [Precision(6, 2)]
        public decimal? sumDpthFhsc { get; set; }

        [Comment("평균 전운량(10분위)")]
        [Precision(6, 2)]
        public decimal? avgTca { get; set; }

        [Comment("평균 중하층운량(10분위)")]
        [Precision(6, 2)]
        public decimal? avgLmac { get; set; }

        [Comment("평균 지면온도(c)")]
        [Precision(6, 2)]
        public decimal? avgTs { get; set; }

        [Comment("최저 초상온도(c)")]
        [Precision(6, 2)]
        public decimal? minTg { get; set; }

        [Comment("평균 5cm 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgCm5Te { get; set; }

        [Comment("평균 10cm 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgCm10Te { get; set; }

        [Comment("평균 20cm 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgCm20Te { get; set; }

        [Comment("평균 30cm 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgCm30Te { get; set; }

        [Comment("평균 0.5m 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgM05Te { get; set; }

        [Comment("평균 1.0m 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgM10Te { get; set; }

        [Comment("평균 1.5m 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgM15Te { get; set; }

        [Comment("평균 3.0m 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgM30Te { get; set; }

        [Comment("평균 5.0m 지중온도(c)")]
        [Precision(6, 2)]
        public decimal? avgM50Te { get; set; }

        [Comment("합계 대형증발량(mm)")]
        [Precision(6, 2)]
        public decimal? sumLrgEv { get; set; }

        [Comment("합계 소형증발량(mm)")]
        [Precision(6, 2)]
        public decimal? sumSmlEv { get; set; }

        [Comment("9-9 강수(mm)")]
        [Precision(6, 2)]
        public decimal? n99Rn { get; set; }

        [Comment("일기현상")]
        [MaxLength(1000)]
        public string iscs { get; set; }

        [Comment("안개 계속 시간(hr)")]
        [Precision(6, 2)]
        public decimal? sumFogDur { get; set; }

        [Comment("변환시간")]
        [MaxLength(20)]
        public string dt { get; set; }
    }


}
