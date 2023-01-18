using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicDBLib.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ASOSCodeList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WPCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WPName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WPMgrName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<decimal>(type: "DECIMAL(10,6)", nullable: false, comment: "위도"),
                    Longitude = table.Column<decimal>(type: "DECIMAL(10,6)", nullable: false, comment: "경도")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASOSCodeList", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ASOSDailyData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tm = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "샘플링 시간"),
                    stnId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점번호")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stnNm = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점명")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgTa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 기온(c)"),
                    minTa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최저 기온(c)"),
                    minTaHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최저기온시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maxTa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최고 기온(c)"),
                    maxTaHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최고기온시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumRnDur = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "강수 계속시간(hr)"),
                    mi10MaxRn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "10분 최다강수량(mm)"),
                    mi10MaxRnHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "10분 최다강수량 시각(mm)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hr1MaxRn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "1시간 최다강수량(mm)"),
                    hr1MaxRnHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "1시간 최다강수량 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumRn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일강수량(mm)"),
                    maxInsWs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최대 순간풍속(m/s)"),
                    maxInsWsWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최대 순간풍속 풍향(16방위)"),
                    maxInsWsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최대 순간풍속 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maxWs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최대 풍속(m/s)"),
                    maxWsWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최대 풍속 풍향(16방위)"),
                    maxWsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최대 풍속 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgWs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 풍속(m/s)"),
                    hr24SumRws = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "풍정합(100m)"),
                    maxWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최다 풍향(16방위)"),
                    avgTd = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 이슬점 온도(c)"),
                    minRhm = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최소 상대습도(%)"),
                    minRhmHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "평균 상대습도 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgRhm = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 상대습도(%)"),
                    avgPv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 증기압(hPa)"),
                    avgPa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 현지기압(hPa)"),
                    maxPs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최고 해면 기압(hPa)"),
                    maxPsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최고 해면기압 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    minPs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최저 해면기압(hPa)"),
                    minPsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최저 해면기압 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgPs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 해면기압(hPa)"),
                    ssDur = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "가조시간(hr)"),
                    sumSsHr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 일조 시간(hr)"),
                    hr1MaxIcsrHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "1시간 최다 일사시각(hhmi)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hr1MaxIcsr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "1시간 최다 일사량(MJ/m2)"),
                    sumGsr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 일사량(MJ/m2)"),
                    ddMefs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일 최심신적설(cm)"),
                    ddMefsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "일 최심신적설 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ddMes = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일 최심적설(cm)"),
                    ddMesHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "일 최심적설 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumDpthFhsc = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 3시간 신적설(cm)"),
                    avgTca = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 전운량(10분위)"),
                    avgLmac = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 중하층운량(10분위)"),
                    avgTs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 지면온도(c)"),
                    minTg = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최저 초상온도(c)"),
                    avgCm5Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 5cm 지중온도(c)"),
                    avgCm10Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 10cm 지중온도(c)"),
                    avgCm20Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 20cm 지중온도(c)"),
                    avgCm30Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 30cm 지중온도(c)"),
                    avgM05Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 0.5m 지중온도(c)"),
                    avgM10Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 1.0m 지중온도(c)"),
                    avgM15Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 1.5m 지중온도(c)"),
                    avgM30Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 3.0m 지중온도(c)"),
                    avgM50Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 5.0m 지중온도(c)"),
                    sumLrgEv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 대형증발량(mm)"),
                    sumSmlEv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 소형증발량(mm)"),
                    n99Rn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "9-9 강수(mm)"),
                    iscs = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true, comment: "일기현상")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumFogDur = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "안개 계속 시간(hr)"),
                    dt = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "변환시간")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASOSDailyData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ASOSHourData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tm = table.Column<DateTime>(type: "DATETIME", unicode: false, nullable: false, comment: "샘플링 시간"),
                    stnId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점 번호")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stnNm = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점명")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ta = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "기온"),
                    taQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "기온 정상여부 판별 정보"),
                    rn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "강수량"),
                    rnQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "강수량 정상여부 판별 정보"),
                    ws = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "풍속"),
                    wsQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "풍속 정상여부 판별 정보"),
                    wd = table.Column<short>(type: "SMALLINT(3)", nullable: true, comment: "풍향"),
                    wdQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "풍향 정상여부 판별 정보"),
                    hm = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "습도"),
                    hmQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "습도 정상여부 판별 정보"),
                    pa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "현지기압"),
                    paQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "현지기압 정상여부 판별 정보"),
                    ps = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "해면기압"),
                    psQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "해면기압 정상여부 판별 정보"),
                    ts = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "지면온도"),
                    tsQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "지면온도 정상여부 판별 정보"),
                    ss = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일조"),
                    sQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "일조 정상여부 판별 정보"),
                    pv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "증기압"),
                    td = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "지면온도"),
                    icsr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일사(MJ/m2)"),
                    dsnw = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "적설(cm)"),
                    hr3Fhsc = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "3시간 신적설(cm)"),
                    dc10Tca = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "전운량"),
                    dc10LmcsCa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "중하층운량"),
                    clfmAbbrCd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "운형"),
                    lcsCh = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최저운고(100m)"),
                    vs = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "시정(10m)"),
                    gndSttCd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "지면상태"),
                    dmstMtphNo = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "현상번호"),
                    m005Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "5cm 지중온도"),
                    m01Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "10cm 지중온도"),
                    m02Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "20cm 지중온도"),
                    m03Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "30cm 지중온도")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASOSHourData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DamCodeList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DamCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DamName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<decimal>(type: "DECIMAL(10,6)", nullable: false, comment: "위도"),
                    Longitude = table.Column<decimal>(type: "DECIMAL(10,6)", nullable: false, comment: "경도")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamCodeList", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DamDailyData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DamCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "댐 코드")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    obsryymtde = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "샘플링 시간"),
                    lowlevel = table.Column<decimal>(type: "DECIMAL(10,4)", nullable: true, comment: "댐 수위"),
                    prcptqy = table.Column<decimal>(type: "DECIMAL(8,2)", nullable: true, comment: "강우량"),
                    inflowqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "유입량"),
                    totdcwtrqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "총 방류량"),
                    rsvwtqy = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: true, comment: "저수량"),
                    rsvwtrt = table.Column<decimal>(type: "DECIMAL(5,1)", nullable: true, comment: "저수율"),
                    dt = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, comment: "MM-dd HH")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamDailyData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DamHourData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DamCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "댐 코드")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    obsrdt = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "샘플링 시간"),
                    lowlevel = table.Column<decimal>(type: "DECIMAL(10,4)", nullable: true, comment: "댐 수위"),
                    rf = table.Column<decimal>(type: "DECIMAL(8,2)", nullable: true, comment: "강우량"),
                    inflowqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "유입량"),
                    totdcwtrqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "총 방류량"),
                    rsvwtqy = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: true, comment: "저수량"),
                    rsvwtrt = table.Column<decimal>(type: "DECIMAL(5,1)", nullable: true, comment: "저수율")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamHourData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JoinDailyData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    obsryymtde = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "샘플링 시간"),
                    DamCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "댐 코드")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lowlevel = table.Column<decimal>(type: "DECIMAL(10,4)", nullable: true, comment: "댐 수위"),
                    prcptqy = table.Column<decimal>(type: "DECIMAL(8,2)", nullable: true, comment: "강우량"),
                    inflowqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "유입량"),
                    totdcwtrqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "총 방류량"),
                    rsvwtqy = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: true, comment: "저수량"),
                    rsvwtrt = table.Column<decimal>(type: "DECIMAL(5,1)", nullable: true, comment: "저수율"),
                    stnId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점번호")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stnNm = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점명")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgTa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 기온(c)"),
                    minTa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최저 기온(c)"),
                    minTaHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최저기온시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maxTa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최고 기온(c)"),
                    maxTaHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최고기온시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumRnDur = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "강수 계속시간(hr)"),
                    mi10MaxRn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "10분 최다강수량(mm)"),
                    mi10MaxRnHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "10분 최다강수량 시각(mm)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hr1MaxRn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "1시간 최다강수량(mm)"),
                    hr1MaxRnHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "1시간 최다강수량 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumRn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일강수량(mm)"),
                    maxInsWs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최대 순간풍속(m/s)"),
                    maxInsWsWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최대 순간풍속 풍향(16방위)"),
                    maxInsWsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최대 순간풍속 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maxWs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최대 풍속(m/s)"),
                    maxWsWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최대 풍속 풍향(16방위)"),
                    maxWsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최대 풍속 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgWs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 풍속(m/s)"),
                    hr24SumRws = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "풍정합(100m)"),
                    maxWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최다 풍향(16방위)"),
                    avgTd = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 이슬점 온도(c)"),
                    minRhm = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최소 상대습도(%)"),
                    minRhmHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "평균 상대습도 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgRhm = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 상대습도(%)"),
                    avgPv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 증기압(hPa)"),
                    avgPa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 현지기압(hPa)"),
                    maxPs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최고 해면 기압(hPa)"),
                    maxPsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최고 해면기압 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    minPs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최저 해면기압(hPa)"),
                    minPsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "최저 해면기압 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgPs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 해면기압(hPa)"),
                    ssDur = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "가조시간(hr)"),
                    sumSsHr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 일조 시간(hr)"),
                    hr1MaxIcsrHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "1시간 최다 일사시각(hhmi)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hr1MaxIcsr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "1시간 최다 일사량(MJ/m2)"),
                    sumGsr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 일사량(MJ/m2)"),
                    ddMefs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일 최심신적설(cm)"),
                    ddMefsHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "일 최심신적설 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ddMes = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일 최심적설(cm)"),
                    ddMesHrmt = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, comment: "일 최심적설 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumDpthFhsc = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 3시간 신적설(cm)"),
                    avgTca = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 전운량(10분위)"),
                    avgLmac = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 중하층운량(10분위)"),
                    avgTs = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 지면온도(c)"),
                    minTg = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "최저 초상온도(c)"),
                    avgCm5Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 5cm 지중온도(c)"),
                    avgCm10Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 10cm 지중온도(c)"),
                    avgCm20Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 20cm 지중온도(c)"),
                    avgCm30Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 30cm 지중온도(c)"),
                    avgM05Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 0.5m 지중온도(c)"),
                    avgM10Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 1.0m 지중온도(c)"),
                    avgM15Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 1.5m 지중온도(c)"),
                    avgM30Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 3.0m 지중온도(c)"),
                    avgM50Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "평균 5.0m 지중온도(c)"),
                    sumLrgEv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 대형증발량(mm)"),
                    sumSmlEv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "합계 소형증발량(mm)"),
                    n99Rn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "9-9 강수(mm)"),
                    iscs = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true, comment: "일기현상")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumFogDur = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "안개 계속 시간(hr)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinDailyData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JoinHourData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    obsrdt = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "샘플링 시간"),
                    DamCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "댐 코드")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lowlevel = table.Column<decimal>(type: "DECIMAL(10,4)", nullable: true, comment: "댐 수위"),
                    rf = table.Column<decimal>(type: "DECIMAL(8,2)", nullable: true, comment: "강우량"),
                    inflowqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "유입량"),
                    totdcwtrqy = table.Column<decimal>(type: "DECIMAL(15,3)", nullable: true, comment: "총 방류량"),
                    rsvwtqy = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: true, comment: "저수량"),
                    rsvwtrt = table.Column<decimal>(type: "DECIMAL(5,1)", nullable: true, comment: "저수율"),
                    stnId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점 번호")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stnNm = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "종관기상관측 지점명")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ta = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "기온"),
                    taQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "기온 정상여부 판별 정보"),
                    rn = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "강수량"),
                    rnQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "강수량 정상여부 판별 정보"),
                    ws = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "풍속"),
                    wsQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "풍속 정상여부 판별 정보"),
                    wd = table.Column<short>(type: "SMALLINT(3)", nullable: true, comment: "풍향"),
                    wdQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "풍향 정상여부 판별 정보"),
                    hm = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "습도"),
                    hmQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "습도 정상여부 판별 정보"),
                    pa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "현지기압"),
                    paQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "현지기압 정상여부 판별 정보"),
                    ps = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "해면기압"),
                    psQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "해면기압 정상여부 판별 정보"),
                    ts = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "지면온도"),
                    tsQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "지면온도 정상여부 판별 정보"),
                    ss = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일조"),
                    sQcflag = table.Column<sbyte>(type: "TINYINT", nullable: false, comment: "일조 정상여부 판별 정보"),
                    pv = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "증기압"),
                    td = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "지면온도"),
                    icsr = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "일사(MJ/m2)"),
                    dsnw = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "적설(cm)"),
                    hr3Fhsc = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "3시간 신적설(cm)"),
                    dc10Tca = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "전운량"),
                    dc10LmcsCa = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "중하층운량"),
                    clfmAbbrCd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "운형"),
                    lcsCh = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최저운고(100m)"),
                    vs = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "시정(10m)"),
                    gndSttCd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "지면상태"),
                    dmstMtphNo = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "현상번호"),
                    m005Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "5cm 지중온도"),
                    m01Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "10cm 지중온도"),
                    m02Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "20cm 지중온도"),
                    m03Te = table.Column<decimal>(type: "DECIMAL(6,2)", nullable: true, comment: "30cm 지중온도")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinHourData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Meta_CheckPoint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TargetCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCheckPoint = table.Column<DateTime>(type: "DateTime", nullable: false),
                    IsComplete = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    DataBeginTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DataLastTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    LastPage = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    RetryCount = table.Column<sbyte>(type: "TINYINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meta_CheckPoint", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Meta_TryCount",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Count = table.Column<int>(type: "INT", nullable: false),
                    TryDate = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meta_TryCount", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServerLogList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LogDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    LogLevel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "TEXT", unicode: false, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerLogList", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ASOSDailyData_stnId_tm",
                table: "ASOSDailyData",
                columns: new[] { "stnId", "tm" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ASOSHourData_stnId_tm",
                table: "ASOSHourData",
                columns: new[] { "stnId", "tm" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DamDailyData_DamCode_obsryymtde",
                table: "DamDailyData",
                columns: new[] { "DamCode", "obsryymtde" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DamHourData_DamCode_obsrdt",
                table: "DamHourData",
                columns: new[] { "DamCode", "obsrdt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinDailyData_DamCode_stnId_obsryymtde",
                table: "JoinDailyData",
                columns: new[] { "DamCode", "stnId", "obsryymtde" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinHourData_DamCode_stnId_obsrdt",
                table: "JoinHourData",
                columns: new[] { "DamCode", "stnId", "obsrdt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meta_CheckPoint_TargetCode",
                table: "Meta_CheckPoint",
                column: "TargetCode");

            migrationBuilder.CreateIndex(
                name: "IX_Meta_TryCount_Name",
                table: "Meta_TryCount",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASOSCodeList");

            migrationBuilder.DropTable(
                name: "ASOSDailyData");

            migrationBuilder.DropTable(
                name: "ASOSHourData");

            migrationBuilder.DropTable(
                name: "DamCodeList");

            migrationBuilder.DropTable(
                name: "DamDailyData");

            migrationBuilder.DropTable(
                name: "DamHourData");

            migrationBuilder.DropTable(
                name: "JoinDailyData");

            migrationBuilder.DropTable(
                name: "JoinHourData");

            migrationBuilder.DropTable(
                name: "Meta_CheckPoint");

            migrationBuilder.DropTable(
                name: "Meta_TryCount");

            migrationBuilder.DropTable(
                name: "ServerLogList");
        }
    }
}
