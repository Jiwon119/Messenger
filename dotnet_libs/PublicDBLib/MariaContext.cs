using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PublicDBLib.MariaDB.ASOS;
using PublicDBLib.MariaDB.Dam;
using PublicDBLib.MariaDB.Join;
using PublicDBLib.MariaDB.Meta;
using PublicDBLib.MariaDB.Reservior;
using System;

namespace PublicDBLib
{
    public class MariaContext : DbContext
    {
        public MariaContext()
        {

        }

        public MariaContext(DbContextOptions<MariaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DamCodeDatum> DamCodeList { get; set; }
        public virtual DbSet<ASOSCodeDatum> ASOSCodeList { get; set; }

        public virtual DbSet<DamDailyDatum> DamDailyData { get; set; }
        public virtual DbSet<DamHourDatum> DamHourData { get; set; }

        public virtual DbSet<ASOSDailyDatum> ASOSDailyData { get; set; }
        public virtual DbSet<ASOSHourDatum> ASOSHourData { get; set; }

        public virtual DbSet<RsvCodeDatum> RsvCodeList { get; set; }
        public virtual DbSet<RsvDailyDatum> RsvDailyData { get; set; }

        public virtual DbSet<JoinDailyDatum> JoinDailyData { get; set; }
        public virtual DbSet<JoinRsvDailyDatum> RsvJoinDailyData { get; set; }
        public virtual DbSet<JoinHourDatum> JoinHourData { get; set; }

        public virtual DbSet<Meta_CheckPoint> CheckPointList { get; set; }
        public virtual DbSet<Meta_TryCount> TryCountList { get; set; }
        public virtual DbSet<Meta_ServerLog> ServerLogList { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder();

                string ip = Environment.GetEnvironmentVariable("PUBLICDATA_DB_IP");
                string port = Environment.GetEnvironmentVariable("PUBLICDATA_DB_PORT");
                connBuilder.Server = ip;
                connBuilder.Port = uint.Parse(port);
                connBuilder.UserID = Environment.GetEnvironmentVariable("PUBLICDATA_DB_USER_ID");
                connBuilder.Password = Environment.GetEnvironmentVariable("PUBLICDATA_DB_PASSWORD");
                connBuilder.Database = Environment.GetEnvironmentVariable("PUBLICDATA_DB_DATABASE");
                connBuilder.CharacterSet = "utf8";

                string connStr = connBuilder.ConnectionString;

                //MariaDbServerVersion
                ServerVersion version = new MariaDbServerVersion(new Version(10, 6));

                optionsBuilder.UseMySql(connStr, version, x => x.EnableRetryOnFailure());
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DamCodeDatum>(entity =>
            {
                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode");

                entity.Property(e => e.DamName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamName");

                entity.Property(e => e.latitude)
                    .HasColumnType("DECIMAL(10,6)")
                    .HasColumnName("Latitude")
                    .HasComment("위도");

                entity.Property(e => e.longitude)
                    .HasColumnType("DECIMAL(10,6)")
                    .HasColumnName("Longitude")
                    .HasComment("경도");

                //entity.Property(e => e.location)
                //    .HasColumnType("POINT")
                //    .HasColumnName("Location")
                //    .HasComment("좌표");
            });

            modelBuilder.Entity<ASOSCodeDatum>(entity =>
            {
                entity.Property(e => e.WPCode)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(10)
                    .HasColumnName("WPCode");

                entity.Property(e => e.WPName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("WPName");

                entity.Property(e => e.WPMgrName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("WPMgrName");

                entity.Property(e => e.latitude)
                    .HasColumnType("DECIMAL(10,6)")
                    .HasColumnName("Latitude")
                    .HasComment("위도");

                entity.Property(e => e.longitude)
                    .HasColumnType("DECIMAL(10,6)")
                    .HasColumnName("Longitude")
                    .HasComment("경도");

                //entity.Property(e => e.location)
                //    .HasColumnType("POINT")
                //    .HasColumnName("Location")
                //    .HasComment("좌표");
            });

            modelBuilder.Entity<ASOSDailyDatum>(entity =>
            {
                entity.Property(e => e.tm)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("tm")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.stnId)
                    .IsRequired()
                    .HasColumnName("stnId")
                    .HasComment("종관기상관측 지점번호")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.stnNm)
                    .IsRequired()
                    .HasColumnName("stnNm")
                    .HasComment("종관기상관측 지점명")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.avgTa).HasColumnName("avgTa")
                    .HasComment("평균 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTa).HasColumnName("minTa")
                    .HasComment("최저 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTaHrmt).HasColumnName("minTaHrmt")
                    .HasComment("최저기온시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.maxTa).HasColumnName("maxTa")
                    .HasComment("최고 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxTaHrmt).HasColumnName("maxTaHrmt")
                    .HasComment("최고기온시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumRnDur).HasColumnName("sumRnDur")
                    .HasComment("강수 계속시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.mi10MaxRn).HasColumnName("mi10MaxRn")
                    .HasComment("10분 최다강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.mi10MaxRnHrmt).HasColumnName("mi10MaxRnHrmt")
                    .HasComment("10분 최다강수량 시각(mm)")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.hr1MaxRn).HasColumnName("hr1MaxRn")
                    .HasComment("1시간 최다강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr1MaxRnHrmt).HasColumnName("hr1MaxRnHrmt")
                    .HasComment("1시간 최다강수량 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumRn).HasColumnName("sumRn")
                    .HasComment("일강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxInsWs).HasColumnName("maxInsWs")
                    .HasComment("최대 순간풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxInsWsWd).HasColumnName("maxInsWsWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최대 순간풍속 풍향(16방위)");

                entity.Property(e => e.maxInsWsHrmt).HasColumnName("maxInsWsHrmt")
                    .HasComment("최대 순간풍속 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.maxWs).HasColumnName("maxWs")
                    .HasComment("최대 풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxWsWd).HasColumnName("maxWsWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최대 풍속 풍향(16방위)");

                entity.Property(e => e.maxWsHrmt).HasColumnName("maxWsHrmt")
                    .HasComment("최대 풍속 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgWs).HasColumnName("avgWs")
                    .HasComment("평균 풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr24SumRws).HasColumnName("hr24SumRws")
                    .HasComment("풍정합(100m)")
                    .HasColumnType("SMALLINT(4)");

                entity.Property(e => e.maxWd).HasColumnName("maxWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최다 풍향(16방위)");

                entity.Property(e => e.avgTd).HasColumnName("avgTd")
                    .HasComment("평균 이슬점 온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minRhm).HasColumnName("minRhm")
                    .HasComment("최소 상대습도(%)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minRhmHrmt).HasColumnName("minRhmHrmt")
                    .HasComment("평균 상대습도 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgRhm).HasColumnName("avgRhm")
                    .HasComment("평균 상대습도(%)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgPv).HasColumnName("avgPv")
                    .HasComment("평균 증기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgPa).HasColumnName("avgPa")
                    .HasComment("평균 현지기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxPs).HasColumnName("maxPs")
                    .HasComment("최고 해면 기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxPsHrmt).HasColumnName("maxPsHrmt")
                    .HasComment("최고 해면기압 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.minPs).HasColumnName("minPs")
                    .HasComment("최저 해면기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minPsHrmt).HasColumnName("minPsHrmt")
                    .HasComment("최저 해면기압 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgPs).HasColumnName("avgPs")
                    .HasComment("평균 해면기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ssDur).HasColumnName("ssDur")
                    .HasComment("가조시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumSsHr).HasColumnName("sumSsHr")
                    .HasComment("합계 일조 시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr1MaxIcsrHrmt).HasColumnName("hr1MaxIcsrHrmt")
                    .HasComment("1시간 최다 일사시각(hhmi)")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.hr1MaxIcsr).HasColumnName("hr1MaxIcsr")
                    .HasComment("1시간 최다 일사량(MJ/m2)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumGsr).HasColumnName("sumGsr")
                    .HasComment("합계 일사량(MJ/m2)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMefs).HasColumnName("ddMefs")
                    .HasComment("일 최심신적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMefsHrmt).HasColumnName("ddMefsHrmt")
                    .HasComment("일 최심신적설 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ddMes).HasColumnName("ddMes")
                    .HasComment("일 최심적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMesHrmt).HasColumnName("ddMesHrmt")
                    .HasComment("일 최심적설 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumDpthFhsc).HasColumnName("sumDpthFhsc")
                    .HasComment("합계 3시간 신적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgTca).HasColumnName("avgTca")
                    .HasComment("평균 전운량(10분위)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgLmac).HasColumnName("avgLmac")
                    .HasComment("평균 중하층운량(10분위)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgTs).HasColumnName("avgTs")
                    .HasComment("평균 지면온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTg).HasColumnName("minTg")
                    .HasComment("최저 초상온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm5Te).HasColumnName("avgCm5Te")
                    .HasComment("평균 5cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm10Te).HasColumnName("avgCm10Te")
                    .HasComment("평균 10cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm20Te).HasColumnName("avgCm20Te")
                    .HasComment("평균 20cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm30Te).HasColumnName("avgCm30Te")
                    .HasComment("평균 30cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM05Te).HasColumnName("avgM05Te")
                    .HasComment("평균 0.5m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM10Te).HasColumnName("avgM10Te")
                    .HasComment("평균 1.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM15Te).HasColumnName("avgM15Te")
                    .HasComment("평균 1.5m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM30Te).HasColumnName("avgM30Te")
                    .HasComment("평균 3.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM50Te).HasColumnName("avgM50Te")
                    .HasComment("평균 5.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumLrgEv).HasColumnName("sumLrgEv")
                    .HasComment("합계 대형증발량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumSmlEv).HasColumnName("sumSmlEv")
                    .HasComment("합계 소형증발량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.n99Rn).HasColumnName("n99Rn")
                    .HasComment("9-9 강수(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.iscs).HasColumnName("iscs")
                    .HasComment("일기현상")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.sumFogDur).HasColumnName("sumFogDur")
                    .HasComment("안개 계속 시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.dt)
                    .IsRequired()
                    .HasColumnName("dt")
                    .HasComment("변환시간")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DamDailyDatum>(entity =>
            {
                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode")
                    .HasComment("댐 코드");

                entity.Property(e => e.obsryymtde)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("obsryymtde")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.lowlevel)
                    .HasColumnType("DECIMAL(10,4)")
                    .HasColumnName("lowlevel")
                    .HasComment("댐 수위");

                entity.Property(e => e.prcptqy)
                    .HasColumnType("DECIMAL(8,2)")
                    .HasColumnName("prcptqy")
                    .HasComment("강우량");

                entity.Property(e => e.inflowqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("inflowqy")
                    .HasComment("유입량");

                entity.Property(e => e.totdcwtrqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("totdcwtrqy")
                    .HasComment("총 방류량");

                entity.Property(e => e.rsvwtqy)
                    .HasColumnType("DECIMAL(18,4)")
                    .HasColumnName("rsvwtqy")
                    .HasComment("저수량");

                entity.Property(e => e.rsvwtrt)
                    .HasColumnType("DECIMAL(5,1)")
                    .HasColumnName("rsvwtrt")
                    .HasComment("저수율");

                entity.Property(e => e.dt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dt")
                    .HasComment("MM-dd HH");
            });

            modelBuilder.Entity<DamHourDatum>(entity =>
            {
                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode")
                    .HasComment("댐 코드");

                entity.Property(e => e.obsrdt)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("obsrdt")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.lowlevel)
                    .HasColumnType("DECIMAL(10,4)")
                    .HasColumnName("lowlevel")
                    .HasComment("댐 수위");

                entity.Property(e => e.rf)
                    .HasColumnType("DECIMAL(8,2)")
                    .HasColumnName("rf")
                    .HasComment("강우량");

                entity.Property(e => e.inflowqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("inflowqy")
                    .HasComment("유입량");

                entity.Property(e => e.totdcwtrqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("totdcwtrqy")
                    .HasComment("총 방류량");

                entity.Property(e => e.rsvwtqy)
                    .HasColumnType("DECIMAL(18,4)")
                    .HasColumnName("rsvwtqy")
                    .HasComment("저수량");

                entity.Property(e => e.rsvwtrt)
                    .HasColumnType("DECIMAL(5,1)")
                    .HasColumnName("rsvwtrt")
                    .HasComment("저수율");
            });

            modelBuilder.Entity<ASOSHourDatum>(entity =>
            {
                entity.Property(e => e.tm)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("tm")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.stnId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("stnId")
                    .HasComment("종관기상관측 지점 번호");

                entity.Property(e => e.stnNm)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("stnNm")
                    .HasComment("종관기상관측 지점명");

                entity.Property(e => e.ta)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ta")
                    .HasComment("기온");

                //null - 정상 , 1 - 오류, 9 - 결측
                entity.Property(e => e.taQcflag)
                    .HasColumnName("taQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("기온 정상여부 판별 정보");

                entity.Property(e => e.rn)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("rn")
                    .HasComment("강수량");

                entity.Property(e => e.rnQcflag)
                    .HasColumnName("rnQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("강수량 정상여부 판별 정보");

                entity.Property(e => e.ws)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ws")
                    .HasComment("풍속");

                entity.Property(e => e.wsQcflag)
                    .HasColumnName("wsQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("풍속 정상여부 판별 정보");

                entity.Property(e => e.wd)
                    .HasColumnType("SMALLINT(3)")
                    .HasColumnName("wd")
                    .HasComment("풍향");

                entity.Property(e => e.wdQcflag)
                    .HasColumnName("wdQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("풍향 정상여부 판별 정보");

                entity.Property(e => e.hm)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("hm")
                    .HasComment("습도");

                entity.Property(e => e.hmQcflag)
                    .HasColumnName("hmQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("습도 정상여부 판별 정보");

                entity.Property(e => e.pa)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("pa")
                    .HasComment("현지기압");

                entity.Property(e => e.paQcflag)
                    .HasColumnName("paQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("현지기압 정상여부 판별 정보");

                entity.Property(e => e.ps)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ps")
                    .HasComment("해면기압");

                entity.Property(e => e.psQcflag)
                    .HasColumnName("psQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("해면기압 정상여부 판별 정보");

                entity.Property(e => e.ss)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ss")
                    .HasComment("일조");

                entity.Property(e => e.ssQcflag)
                    .HasColumnName("sQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("일조 정상여부 판별 정보");

                entity.Property(e => e.ts)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ts")
                    .HasComment("지면온도");

                entity.Property(e => e.tsQcflag)
                    .HasColumnName("tsQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("지면온도 정상여부 판별 정보");

                entity.Property(e => e.pv)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("pv")
                    .HasComment("증기압");

                entity.Property(e => e.td)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("td")
                    .HasComment("지면온도");


                entity.Property(e => e.icsr)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("icsr")
                    .HasComment("일사(MJ/m2)");

                entity.Property(e => e.dsnw)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dsnw")
                    .HasComment("적설(cm)");

                entity.Property(e => e.hr3Fhsc)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("hr3Fhsc")
                    .HasComment("3시간 신적설(cm)");

                entity.Property(e => e.dc10Tca)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dc10Tca")
                    .HasComment("전운량");

                entity.Property(e => e.dc10LmcsCa)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dc10LmcsCa")
                    .HasComment("중하층운량");

                entity.Property(e => e.clfmAbbrCd)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("clfmAbbrCd")
                    .HasComment("운형");

                entity.Property(e => e.lcsCh)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("lcsCh")
                    .HasComment("최저운고(100m)");

                entity.Property(e => e.vs)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("vs")
                    .HasComment("시정(10m)");

                entity.Property(e => e.gndSttCd)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("gndSttCd")
                    .HasComment("지면상태");

                entity.Property(e => e.dmstMtphNo)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("dmstMtphNo")
                    .HasComment("현상번호");

                entity.Property(e => e.m005Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m005Te")
                    .HasComment("5cm 지중온도");

                entity.Property(e => e.m01Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m01Te")
                    .HasComment("10cm 지중온도");

                entity.Property(e => e.m02Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m02Te")
                    .HasComment("20cm 지중온도");

                entity.Property(e => e.m03Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m03Te")
                    .HasComment("30cm 지중온도");
            });

            modelBuilder.Entity<Meta_ServerLog>(entity =>
            {
                entity.Property(e => e.LogDate)
                    .HasColumnType("DateTime")
                    .HasColumnName("LogDate")
                    .IsRequired();

                entity.Property(e => e.LogLevel)
                    .IsRequired()
                    .HasColumnName("LogLevel")
                    .IsUnicode(false)
                    .HasMaxLength(20);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasColumnName("Message")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Meta_CheckPoint>(entity =>
            {
                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasColumnName("Category")
                    .IsUnicode(false)
                    .HasMaxLength(20);

                entity.Property(e => e.TargetCode)
                    .IsRequired()
                    .HasColumnName("TargetCode")
                    .IsUnicode(false)
                    .HasMaxLength(20);

                entity.Property(e => e.DateCheckPoint)
                    .HasColumnType("DateTime")
                    .HasColumnName("DateCheckPoint")
                    .IsRequired();

                entity.Property(e => e.IsComplete)
                    .IsRequired()
                    .HasColumnType("TINYINT")
                    .HasColumnName("IsComplete");

                entity.Property(e => e.DataBeginTime)
                    .HasColumnType("DateTime")
                    .HasColumnName("DataBeginTime")
                    .IsRequired();

                entity.Property(e => e.DataLastTime)
                    .HasColumnType("DateTime")
                    .HasColumnName("DataLastTime")
                    .IsRequired();

                entity.Property(e => e.LastPage)
                    .IsRequired()
                    .HasColumnType("TINYINT")
                    .HasColumnName("LastPage");

                entity.Property(e => e.RetryCount)
                    .IsRequired()
                    .HasColumnType("TINYINT")
                    .HasColumnName("RetryCount");

            });

            modelBuilder.Entity<Meta_TryCount>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("Name")
                    .IsUnicode(false)
                    .HasMaxLength(100);

                entity.Property(e => e.Count)
                    .IsRequired()
                    .HasColumnType("INT")
                    .HasColumnName("Count");

                entity.Property(e => e.TryDate)
                    .HasColumnType("DateTime")
                    .HasColumnName("TryDate")
                    .IsRequired();
            });

            modelBuilder.Entity<JoinDailyDatum>(entity =>
            {
                entity.Property(e => e.obsryymtde)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("obsryymtde")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode")
                    .HasComment("댐 코드");

                entity.Property(e => e.lowlevel)
                    .HasColumnType("DECIMAL(10,4)")
                    .HasColumnName("lowlevel")
                    .HasComment("댐 수위");

                entity.Property(e => e.prcptqy)
                    .HasColumnType("DECIMAL(8,2)")
                    .HasColumnName("prcptqy")
                    .HasComment("강우량");

                entity.Property(e => e.inflowqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("inflowqy")
                    .HasComment("유입량");

                entity.Property(e => e.totdcwtrqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("totdcwtrqy")
                    .HasComment("총 방류량");

                entity.Property(e => e.rsvwtqy)
                    .HasColumnType("DECIMAL(18,4)")
                    .HasColumnName("rsvwtqy")
                    .HasComment("저수량");

                entity.Property(e => e.rsvwtrt)
                    .HasColumnType("DECIMAL(5,1)")
                    .HasColumnName("rsvwtrt")
                    .HasComment("저수율");


                entity.Property(e => e.stnId)
                .IsRequired()
                    .HasColumnName("stnId")
                    .HasComment("종관기상관측 지점번호")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.stnNm)
                    .IsRequired()
                    .HasColumnName("stnNm")
                    .HasComment("종관기상관측 지점명")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.avgTa).HasColumnName("avgTa")
                    .HasComment("평균 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTa).HasColumnName("minTa")
                    .HasComment("최저 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTaHrmt).HasColumnName("minTaHrmt")
                    .HasComment("최저기온시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.maxTa).HasColumnName("maxTa")
                    .HasComment("최고 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxTaHrmt).HasColumnName("maxTaHrmt")
                    .HasComment("최고기온시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumRnDur).HasColumnName("sumRnDur")
                    .HasComment("강수 계속시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.mi10MaxRn).HasColumnName("mi10MaxRn")
                    .HasComment("10분 최다강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.mi10MaxRnHrmt).HasColumnName("mi10MaxRnHrmt")
                    .HasComment("10분 최다강수량 시각(mm)")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.hr1MaxRn).HasColumnName("hr1MaxRn")
                    .HasComment("1시간 최다강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr1MaxRnHrmt).HasColumnName("hr1MaxRnHrmt")
                    .HasComment("1시간 최다강수량 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumRn).HasColumnName("sumRn")
                    .HasComment("일강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxInsWs).HasColumnName("maxInsWs")
                    .HasComment("최대 순간풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxInsWsWd).HasColumnName("maxInsWsWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최대 순간풍속 풍향(16방위)");

                entity.Property(e => e.maxInsWsHrmt).HasColumnName("maxInsWsHrmt")
                    .HasComment("최대 순간풍속 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.maxWs).HasColumnName("maxWs")
                    .HasComment("최대 풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxWsWd).HasColumnName("maxWsWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최대 풍속 풍향(16방위)");

                entity.Property(e => e.maxWsHrmt).HasColumnName("maxWsHrmt")
                    .HasComment("최대 풍속 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgWs).HasColumnName("avgWs")
                    .HasComment("평균 풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr24SumRws).HasColumnName("hr24SumRws")
                    .HasComment("풍정합(100m)")
                    .HasColumnType("SMALLINT(4)");

                entity.Property(e => e.maxWd).HasColumnName("maxWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최다 풍향(16방위)");

                entity.Property(e => e.avgTd).HasColumnName("avgTd")
                    .HasComment("평균 이슬점 온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minRhm).HasColumnName("minRhm")
                    .HasComment("최소 상대습도(%)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minRhmHrmt).HasColumnName("minRhmHrmt")
                    .HasComment("평균 상대습도 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgRhm).HasColumnName("avgRhm")
                    .HasComment("평균 상대습도(%)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgPv).HasColumnName("avgPv")
                    .HasComment("평균 증기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgPa).HasColumnName("avgPa")
                    .HasComment("평균 현지기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxPs).HasColumnName("maxPs")
                    .HasComment("최고 해면 기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxPsHrmt).HasColumnName("maxPsHrmt")
                    .HasComment("최고 해면기압 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.minPs).HasColumnName("minPs")
                    .HasComment("최저 해면기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minPsHrmt).HasColumnName("minPsHrmt")
                    .HasComment("최저 해면기압 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgPs).HasColumnName("avgPs")
                    .HasComment("평균 해면기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ssDur).HasColumnName("ssDur")
                    .HasComment("가조시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumSsHr).HasColumnName("sumSsHr")
                    .HasComment("합계 일조 시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr1MaxIcsrHrmt).HasColumnName("hr1MaxIcsrHrmt")
                    .HasComment("1시간 최다 일사시각(hhmi)")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.hr1MaxIcsr).HasColumnName("hr1MaxIcsr")
                    .HasComment("1시간 최다 일사량(MJ/m2)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumGsr).HasColumnName("sumGsr")
                    .HasComment("합계 일사량(MJ/m2)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMefs).HasColumnName("ddMefs")
                    .HasComment("일 최심신적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMefsHrmt).HasColumnName("ddMefsHrmt")
                    .HasComment("일 최심신적설 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ddMes).HasColumnName("ddMes")
                    .HasComment("일 최심적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMesHrmt).HasColumnName("ddMesHrmt")
                    .HasComment("일 최심적설 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumDpthFhsc).HasColumnName("sumDpthFhsc")
                    .HasComment("합계 3시간 신적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgTca).HasColumnName("avgTca")
                    .HasComment("평균 전운량(10분위)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgLmac).HasColumnName("avgLmac")
                    .HasComment("평균 중하층운량(10분위)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgTs).HasColumnName("avgTs")
                    .HasComment("평균 지면온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTg).HasColumnName("minTg")
                    .HasComment("최저 초상온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm5Te).HasColumnName("avgCm5Te")
                    .HasComment("평균 5cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm10Te).HasColumnName("avgCm10Te")
                    .HasComment("평균 10cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm20Te).HasColumnName("avgCm20Te")
                    .HasComment("평균 20cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm30Te).HasColumnName("avgCm30Te")
                    .HasComment("평균 30cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM05Te).HasColumnName("avgM05Te")
                    .HasComment("평균 0.5m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM10Te).HasColumnName("avgM10Te")
                    .HasComment("평균 1.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM15Te).HasColumnName("avgM15Te")
                    .HasComment("평균 1.5m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM30Te).HasColumnName("avgM30Te")
                    .HasComment("평균 3.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM50Te).HasColumnName("avgM50Te")
                    .HasComment("평균 5.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumLrgEv).HasColumnName("sumLrgEv")
                    .HasComment("합계 대형증발량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumSmlEv).HasColumnName("sumSmlEv")
                    .HasComment("합계 소형증발량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.n99Rn).HasColumnName("n99Rn")
                    .HasComment("9-9 강수(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.iscs).HasColumnName("iscs")
                    .HasComment("일기현상")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.sumFogDur).HasColumnName("sumFogDur")
                    .HasComment("안개 계속 시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");
            });

            modelBuilder.Entity<JoinHourDatum>(entity =>
            {
                entity.Property(e => e.obsrdt)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("obsrdt")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode")
                    .HasComment("댐 코드");

                entity.Property(e => e.lowlevel)
                    .HasColumnType("DECIMAL(10,4)")
                    .HasColumnName("lowlevel")
                    .HasComment("댐 수위");

                entity.Property(e => e.rf)
                    .HasColumnType("DECIMAL(8,2)")
                    .HasColumnName("rf")
                    .HasComment("강우량");

                entity.Property(e => e.inflowqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("inflowqy")
                    .HasComment("유입량");

                entity.Property(e => e.totdcwtrqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("totdcwtrqy")
                    .HasComment("총 방류량");

                entity.Property(e => e.rsvwtqy)
                    .HasColumnType("DECIMAL(18,4)")
                    .HasColumnName("rsvwtqy")
                    .HasComment("저수량");

                entity.Property(e => e.rsvwtrt)
                    .HasColumnType("DECIMAL(5,1)")
                    .HasColumnName("rsvwtrt")
                    .HasComment("저수율");

                entity.Property(e => e.stnId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("stnId")
                    .HasComment("종관기상관측 지점 번호");

                entity.Property(e => e.stnNm)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("stnNm")
                    .HasComment("종관기상관측 지점명");

                entity.Property(e => e.ta)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ta")
                    .HasComment("기온");

                //null - 정상 , 1 - 오류, 9 - 결측
                entity.Property(e => e.taQcflag)
                    .HasColumnName("taQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("기온 정상여부 판별 정보");

                entity.Property(e => e.rn)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("rn")
                    .HasComment("강수량");

                entity.Property(e => e.rnQcflag)
                    .HasColumnName("rnQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("강수량 정상여부 판별 정보");

                entity.Property(e => e.ws)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ws")
                    .HasComment("풍속");

                entity.Property(e => e.wsQcflag)
                    .HasColumnName("wsQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("풍속 정상여부 판별 정보");

                entity.Property(e => e.wd)
                    .HasColumnType("SMALLINT(3)")
                    .HasColumnName("wd")
                    .HasComment("풍향");

                entity.Property(e => e.wdQcflag)
                    .HasColumnName("wdQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("풍향 정상여부 판별 정보");

                entity.Property(e => e.hm)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("hm")
                    .HasComment("습도");

                entity.Property(e => e.hmQcflag)
                    .HasColumnName("hmQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("습도 정상여부 판별 정보");

                entity.Property(e => e.pa)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("pa")
                    .HasComment("현지기압");

                entity.Property(e => e.paQcflag)
                    .HasColumnName("paQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("현지기압 정상여부 판별 정보");

                entity.Property(e => e.ps)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ps")
                    .HasComment("해면기압");

                entity.Property(e => e.psQcflag)
                    .HasColumnName("psQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("해면기압 정상여부 판별 정보");

                entity.Property(e => e.ss)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ss")
                    .HasComment("일조");

                entity.Property(e => e.ssQcflag)
                    .HasColumnName("sQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("일조 정상여부 판별 정보");

                entity.Property(e => e.ts)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ts")
                    .HasComment("지면온도");

                entity.Property(e => e.tsQcflag)
                    .HasColumnName("tsQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("지면온도 정상여부 판별 정보");

                entity.Property(e => e.pv)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("pv")
                    .HasComment("증기압");

                entity.Property(e => e.td)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("td")
                    .HasComment("지면온도");


                entity.Property(e => e.icsr)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("icsr")
                    .HasComment("일사(MJ/m2)");

                entity.Property(e => e.dsnw)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dsnw")
                    .HasComment("적설(cm)");

                entity.Property(e => e.hr3Fhsc)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("hr3Fhsc")
                    .HasComment("3시간 신적설(cm)");

                entity.Property(e => e.dc10Tca)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dc10Tca")
                    .HasComment("전운량");

                entity.Property(e => e.dc10LmcsCa)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dc10LmcsCa")
                    .HasComment("중하층운량");

                entity.Property(e => e.clfmAbbrCd)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("clfmAbbrCd")
                    .HasComment("운형");

                entity.Property(e => e.lcsCh)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("lcsCh")
                    .HasComment("최저운고(100m)");

                entity.Property(e => e.vs)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("vs")
                    .HasComment("시정(10m)");

                entity.Property(e => e.gndSttCd)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("gndSttCd")
                    .HasComment("지면상태");

                entity.Property(e => e.dmstMtphNo)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("dmstMtphNo")
                    .HasComment("현상번호");

                entity.Property(e => e.m005Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m005Te")
                    .HasComment("5cm 지중온도");

                entity.Property(e => e.m01Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m01Te")
                    .HasComment("10cm 지중온도");

                entity.Property(e => e.m02Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m02Te")
                    .HasComment("20cm 지중온도");

                entity.Property(e => e.m03Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m03Te")
                    .HasComment("30cm 지중온도");
            });

        }

        #region Backup
        /*
		private void backup()
        {
            ModelBuilder modelBuilder = null;
            modelBuilder.Entity<DamCodeDatum>(entity =>
            {
                entity
                .HasKey(e => e.Id)
                .HasName("DamCode_PK");

                entity.ToTable("DamCodeList");

                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode");

                entity.Property(e => e.DamName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamName");
            });

            modelBuilder.Entity<ASOSCodeDatum>(entity =>
            {
                entity
                .HasKey(e => e.Id)
                .HasName("WPCode_PK");

                entity.HasIndex(e => e.WPCode);

                entity.ToTable("ASOSCodeList");

                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.WPCode)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(10)
                    .HasColumnName("WPCode");

                entity.Property(e => e.WPName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("WPName");

                entity.Property(e => e.WPMgrName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("WPMgrName");
            });

            modelBuilder.Entity<DamSpec>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("DamSpecKey");

                entity.HasIndex(e => e.damnm);

                entity.ToTable("DamSpecList");

                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.competDe)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("competDe")
                    .HasComment("완료일");

                entity.Property(e => e.damFom)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("damFom")
                    .HasComment("형식");

                entity.Property(e => e.damdgr)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("damdgr")
                    .HasComment("하천");

                entity.Property(e => e.damnm)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("damnm")
                    .HasComment("댐이름");

                entity.Property(e => e.floodadjstCpcty)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("floodadjstCpcty")
                    .HasComment("홍수조절용량(106m3)");

                entity.Property(e => e.floodseLmttWal)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("floodseLmttWal")
                    .HasComment("상시만수위(m)");

                entity.Property(e => e.fyerUswtrsuplyplanqy)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("fyerUswtrsuplyplanqy")
                    .HasComment("연간용수공급량(106m3)");

                entity.Property(e => e.hg)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("hg")
                    .HasComment("높이(m)");

                entity.Property(e => e.lowlevel)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("lowlevel")
                    .HasComment("저수위(m)");

                entity.Property(e => e.lt)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("lt")
                    .HasComment("길이(m)");

                entity.Property(e => e.nrmltAl)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("nrmltAl")
                    .HasComment("정상표고(EL.m)");

                entity.Property(e => e.ordtmFwal)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("ordtmFwal")
                    .HasComment("홍수기제한수위(m)");

                entity.Property(e => e.planFwal)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("planFwal")
                    .HasComment("계획홍수위(m)");

                entity.Property(e => e.rsvwtAr)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("rsvwtAr")
                    .HasComment("저수면적");

                entity.Property(e => e.strwrkDe)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("strwrkDe")
                    .HasComment("사업기간");

                entity.Property(e => e.totRsvwtcpcty)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("totRsvwtcpcty")
                    .HasComment("총저수용량(106m3)");

                entity.Property(e => e.validRsvwtcpcty)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("validRsvwtcpcty")
                    .HasComment("유효저수용량(106m3)");

                entity.Property(e => e.vl)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("vl")
                    .HasComment("체적(1000m3)");

                entity.Property(e => e.wollyupvlAl)
                    .IsRequired()
                    .HasColumnType("DOUBLE")
                    .HasColumnName("wollyupvlAl")
                    .HasComment("월류정표고");
            });

            modelBuilder.Entity<DamDailyDatum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("DamDailyData");
                entity.HasIndex(e => e.DamCode);

                entity.Property(e => e.Id)
                    .HasColumnType("BIGINT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode")
                    .HasComment("댐 코드");

                entity.Property(e => e.obsryymtde)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("obsryymtde")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.lowlevel)
                    .HasColumnType("DECIMAL(10,4)")
                    .HasColumnName("lowlevel")
                    .HasComment("댐 수위");

                entity.Property(e => e.prcptqy)
                    .HasColumnType("DECIMAL(8,2)")
                    .HasColumnName("prcptqy")
                    .HasComment("강우량");

                entity.Property(e => e.inflowqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("inflowqy")
                    .HasComment("유입량");

                entity.Property(e => e.totdcwtrqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("totdcwtrqy")
                    .HasComment("총 방류량");

                entity.Property(e => e.rsvwtqy)
                    .HasColumnType("DECIMAL(18,4)")
                    .HasColumnName("rsvwtqy")
                    .HasComment("저수량");

                entity.Property(e => e.rsvwtrt)
                    .HasColumnType("DECIMAL(5,1)")
                    .HasColumnName("rsvwtrt")
                    .HasComment("저수율");

                entity.Property(e => e.dt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dt")
                    .HasComment("MM-dd HH");
            });

            modelBuilder.Entity<DamHourDatum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("DamHourData");
                entity.HasIndex(e => e.DamCode);

                entity.Property(e => e.Id)
                    .HasColumnType("BIGINT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DamCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DamCode")
                    .HasComment("댐 코드");

                entity.Property(e => e.obsrdt)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("obsrdt")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.lowlevel)
                    .HasColumnType("DECIMAL(10,4)")
                    .HasColumnName("lowlevel")
                    .HasComment("댐 수위");

                entity.Property(e => e.rf)
                    .HasColumnType("DECIMAL(8,2)")
                    .HasColumnName("rf")
                    .HasComment("강우량");

                entity.Property(e => e.inflowqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("inflowqy")
                    .HasComment("유입량");

                entity.Property(e => e.totdcwtrqy)
                    .HasColumnType("DECIMAL(15,3)")
                    .HasColumnName("totdcwtrqy")
                    .HasComment("총 방류량");

                entity.Property(e => e.rsvwtqy)
                    .HasColumnType("DECIMAL(18,4)")
                    .HasColumnName("rsvwtqy")
                    .HasComment("저수량");

                entity.Property(e => e.rsvwtrt)
                    .HasColumnType("DECIMAL(5,1)")
                    .HasColumnName("rsvwtrt")
                    .HasComment("저수율");

                entity.Property(e => e.dt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("dt")
                    .HasComment("MM-dd HH");
            });

            modelBuilder.Entity<ASOSDailyDatum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.stnId);
                entity.ToTable("ASOSDailyData");

                entity.Property(e => e.Id)
                    .HasColumnType("BIGINT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.tm)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("tm")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.stnId)
                .IsRequired()
                    .HasColumnName("stnId")
                    .HasComment("종관기상관측 지점번호")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.stnNm)
                    .IsRequired()
                    .HasColumnName("stnNm")
                    .HasComment("종관기상관측 지점명")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.avgTa).HasColumnName("avgTa")
                    .HasComment("평균 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTa).HasColumnName("minTa")
                    .HasComment("최저 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTaHrmt).HasColumnName("minTaHrmt")
                    .HasComment("최저기온시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.maxTa).HasColumnName("maxTa")
                    .HasComment("최고 기온(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxTaHrmt).HasColumnName("maxTaHrmt")
                    .HasComment("최고기온시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumRnDur).HasColumnName("sumRnDur")
                    .HasComment("강수 계속시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.mi10MaxRn).HasColumnName("mi10MaxRn")
                    .HasComment("10분 최다강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.mi10MaxRnHrmt).HasColumnName("mi10MaxRnHrmt")
                    .HasComment("10분 최다강수량 시각(mm)")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.hr1MaxRn).HasColumnName("hr1MaxRn")
                    .HasComment("1시간 최다강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr1MaxRnHrmt).HasColumnName("hr1MaxRnHrmt")
                    .HasComment("1시간 최다강수량 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumRn).HasColumnName("sumRn")
                    .HasComment("일강수량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxInsWs).HasColumnName("maxInsWs")
                    .HasComment("최대 순간풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxInsWsWd).HasColumnName("maxInsWsWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최대 순간풍속 풍향(16방위)");

                entity.Property(e => e.maxInsWsHrmt).HasColumnName("maxInsWsHrmt")
                    .HasComment("최대 순간풍속 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.maxWs).HasColumnName("maxWs")
                    .HasComment("최대 풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxWsWd).HasColumnName("maxWsWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최대 풍속 풍향(16방위)");

                entity.Property(e => e.maxWsHrmt).HasColumnName("maxWsHrmt")
                    .HasComment("최대 풍속 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgWs).HasColumnName("avgWs")
                    .HasComment("평균 풍속(m/s)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr24SumRws).HasColumnName("hr24SumRws")
                    .HasComment("풍정합(100m)")
                    .HasColumnType("SMALLINT(4)");

                entity.Property(e => e.maxWd).HasColumnName("maxWd")
                    .HasColumnType("SMALLINT(4)")
                    .HasComment("최다 풍향(16방위)");

                entity.Property(e => e.avgTd).HasColumnName("avgTd")
                    .HasComment("평균 이슬점 온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minRhm).HasColumnName("minRhm")
                    .HasComment("최소 상대습도(%)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minRhmHrmt).HasColumnName("minRhmHrmt")
                    .HasComment("평균 상대습도 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgRhm).HasColumnName("avgRhm")
                    .HasComment("평균 상대습도(%)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgPv).HasColumnName("avgPv")
                    .HasComment("평균 증기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgPa).HasColumnName("avgPa")
                    .HasComment("평균 현지기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxPs).HasColumnName("maxPs")
                    .HasComment("최고 해면 기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.maxPsHrmt).HasColumnName("maxPsHrmt")
                    .HasComment("최고 해면기압 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.minPs).HasColumnName("minPs")
                    .HasComment("최저 해면기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minPsHrmt).HasColumnName("minPsHrmt")
                    .HasComment("최저 해면기압 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.avgPs).HasColumnName("avgPs")
                    .HasComment("평균 해면기압(hPa)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ssDur).HasColumnName("ssDur")
                    .HasComment("가조시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumSsHr).HasColumnName("sumSsHr")
                    .HasComment("합계 일조 시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.hr1MaxIcsrHrmt).HasColumnName("hr1MaxIcsrHrmt")
                    .HasComment("1시간 최다 일사시각(hhmi)")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.hr1MaxIcsr).HasColumnName("hr1MaxIcsr")
                    .HasComment("1시간 최다 일사량(MJ/m2)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumGsr).HasColumnName("sumGsr")
                    .HasComment("합계 일사량(MJ/m2)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMefs).HasColumnName("ddMefs")
                    .HasComment("일 최심신적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMefsHrmt).HasColumnName("ddMefsHrmt")
                    .HasComment("일 최심신적설 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ddMes).HasColumnName("ddMes")
                    .HasComment("일 최심적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.ddMesHrmt).HasColumnName("ddMesHrmt")
                    .HasComment("일 최심적설 시각")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.sumDpthFhsc).HasColumnName("sumDpthFhsc")
                    .HasComment("합계 3시간 신적설(cm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgTca).HasColumnName("avgTca")
                    .HasComment("평균 전운량(10분위)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgLmac).HasColumnName("avgLmac")
                    .HasComment("평균 중하층운량(10분위)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgTs).HasColumnName("avgTs")
                    .HasComment("평균 지면온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.minTg).HasColumnName("minTg")
                    .HasComment("최저 초상온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm5Te).HasColumnName("avgCm5Te")
                    .HasComment("평균 5cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm10Te).HasColumnName("avgCm10Te")
                    .HasComment("평균 10cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm20Te).HasColumnName("avgCm20Te")
                    .HasComment("평균 20cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgCm30Te).HasColumnName("avgCm30Te")
                    .HasComment("평균 30cm 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM05Te).HasColumnName("avgM05Te")
                    .HasComment("평균 0.5m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM10Te).HasColumnName("avgM10Te")
                    .HasComment("평균 1.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM15Te).HasColumnName("avgM15Te")
                    .HasComment("평균 1.5m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM30Te).HasColumnName("avgM30Te")
                    .HasComment("평균 3.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.avgM50Te).HasColumnName("avgM50Te")
                    .HasComment("평균 5.0m 지중온도(c)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumLrgEv).HasColumnName("sumLrgEv")
                    .HasComment("합계 대형증발량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.sumSmlEv).HasColumnName("sumSmlEv")
                    .HasComment("합계 소형증발량(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.n99Rn).HasColumnName("n99Rn")
                    .HasComment("9-9 강수(mm)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.iscs).HasColumnName("iscs")
                    .HasComment("일기현상")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.sumFogDur).HasColumnName("sumFogDur")
                    .HasComment("안개 계속 시간(hr)")
                    .HasColumnType("DECIMAL(6,2)");

                entity.Property(e => e.dt).IsRequired().HasColumnName("dt")
                    .HasComment("변환시간")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ASOSHourDatum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.stnId);
                entity.ToTable("ASOSHourData");

                entity.Property(e => e.Id)
                    .HasColumnType("BIGINT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.tm)
                    .HasColumnType("DATETIME")
                    .IsRequired()
                    .HasColumnName("tm")
                    .HasComment("샘플링 시간");

                entity.Property(e => e.stnId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("stnId")
                    .HasComment("종관기상관측 지점 번호");

                entity.Property(e => e.stnNm)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("stnNm")
                    .HasComment("종관기상관측 지점명");

                entity.Property(e => e.ta)

                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ta")
                    .HasComment("기온");

                //null - 정상 , 1 - 오류, 9 - 결측
                entity.Property(e => e.taQcflag)
                    .HasColumnName("taQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("기온 정상여부 판별 정보");

                entity.Property(e => e.rn)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("rn")
                    .HasComment("강수량");

                entity.Property(e => e.rnQcflag)
                    .HasColumnName("rnQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("강수량 정상여부 판별 정보");

                entity.Property(e => e.ws)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ws")
                    .HasComment("풍속");

                entity.Property(e => e.wsQcflag)
                    .HasColumnName("wsQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("풍속 정상여부 판별 정보");

                entity.Property(e => e.wd)
                    .HasColumnType("SMALLINT(3)")
                    .HasColumnName("wd")
                    .HasComment("풍향");

                entity.Property(e => e.wdQcflag)
                    .HasColumnName("wdQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("풍향 정상여부 판별 정보");

                entity.Property(e => e.hm)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("hm")
                    .HasComment("습도");

                entity.Property(e => e.hmQcflag)
                    .HasColumnName("hmQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("습도 정상여부 판별 정보");

                entity.Property(e => e.pa)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("pa")
                    .HasComment("현지기압");

                entity.Property(e => e.paQcflag)
                    .HasColumnName("paQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("현지기압 정상여부 판별 정보");

                entity.Property(e => e.ps)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ps")
                    .HasComment("해면기압");

                entity.Property(e => e.psQcflag)
                    .HasColumnName("psQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("해면기압 정상여부 판별 정보");

                entity.Property(e => e.ss)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ss")
                    .HasComment("일조");

                entity.Property(e => e.ssQcflag)
                    .HasColumnName("sQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("일조 정상여부 판별 정보");

                entity.Property(e => e.ts)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("ts")
                    .HasComment("지면온도");

                entity.Property(e => e.tsQcflag)
                    .HasColumnName("tsQcflag")
                    .HasColumnType("TINYINT")
                    .HasComment("지면온도 정상여부 판별 정보");

                entity.Property(e => e.pv)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("pv")
                    .HasComment("증기압");

                entity.Property(e => e.td)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("td")
                    .HasComment("지면온도");


                entity.Property(e => e.icsr)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("icsr")
                    .HasComment("일사(MJ/m2)");

                entity.Property(e => e.dsnw)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dsnw")
                    .HasComment("적설(cm)");

                entity.Property(e => e.hr3Fhsc)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("hr3Fhsc")
                    .HasComment("3시간 신적설(cm)");

                entity.Property(e => e.dc10Tca)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dc10Tca")
                    .HasComment("전운량");

                entity.Property(e => e.dc10LmcsCa)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("dc10LmcsCa")
                    .HasComment("중하층운량");

                entity.Property(e => e.clfmAbbrCd)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("clfmAbbrCd")
                    .HasComment("운형");

                entity.Property(e => e.lcsCh)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("lcsCh")
                    .HasComment("최저운고(100m)");

                entity.Property(e => e.vs)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("vs")
                    .HasComment("시정(10m)");

                entity.Property(e => e.gndSttCd)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("gndSttCd")
                    .HasComment("지면상태");

                entity.Property(e => e.dmstMtphNo)
                    .HasColumnType("SMALLINT(4)")
                    .HasColumnName("dmstMtphNo")
                    .HasComment("현상번호");

                entity.Property(e => e.m005Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m005Te")
                    .HasComment("5cm 지중온도");

                entity.Property(e => e.m01Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m01Te")
                    .HasComment("10cm 지중온도");

                entity.Property(e => e.m02Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m02Te")
                    .HasComment("20cm 지중온도");

                entity.Property(e => e.m03Te)
                    .HasColumnType("DECIMAL(6,2)")
                    .HasColumnName("m03Te")
                    .HasComment("30cm 지중온도");
            });

            modelBuilder.Entity<Meta_ServerLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("ServerLogList");

                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LogDate)
                    .HasColumnType("DateTime")
                    .HasColumnName("LogDate")
                    .IsRequired();

                entity.Property(e => e.LogLevel)
                    .IsRequired()
                    .HasColumnName("LogLevel")
                    .IsUnicode(false)
                    .HasMaxLength(20);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasColumnName("Message")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Meta_TryCount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Meta_TryCount");
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id)
                    .HasColumnType("INT")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("Name")
                    .IsUnicode(false)
                    .HasMaxLength(100);

                entity.Property(e => e.Count)
                    .IsRequired()
                    .HasColumnType("INT")
                    .HasColumnName("Count");

                entity.Property(e => e.TryDate)
                    .HasColumnType("DateTime")
                    .HasColumnName("TryDate")
                    .IsRequired();
            });
        }
        */
        #endregion
    }

}

