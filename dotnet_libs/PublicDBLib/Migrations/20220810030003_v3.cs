using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicDBLib.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JoinRsvDailyData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacCode = table.Column<string>(type: "varchar(255)", nullable: false, comment: "저수지코드")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FacName = table.Column<string>(type: "longtext", nullable: true, comment: "저수지이름")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: true, comment: "저수지위치")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CheckDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "측정날짜"),
                    WaterLevel = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "저수지수위"),
                    Rate = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "저수율"),
                    stnId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "종관기상관측 지점번호")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stnNm = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "종관기상관측 지점명")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgTa = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균기온(c)"),
                    minTa = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최저기온(c)"),
                    minTaHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "최저기온시간")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maxTa = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최고기온(c)"),
                    maxTaHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "최고기온시간")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumRnDur = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "강수계속시간"),
                    mi10MaxRn = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "10분 최다강수량(mm)"),
                    mi10MaxRnHrmt = table.Column<string>(type: "longtext", precision: 6, scale: 2, nullable: true, comment: "10분 최다강수량 시각(mm)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hr1MaxRn = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "1시간 최다강수량(mm)"),
                    hr1MaxRnHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "1시간 최다강수량 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumRn = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "일강수량(mm)"),
                    maxInsWs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최대 순간풍속(m/s)"),
                    maxInsWsWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최대 순간풍속 풍향(16방위)"),
                    maxInsWsHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "최대 순간풍속 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maxWs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최대 풍속(m/s)"),
                    maxWsWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최대 충속 풍향(16방위)"),
                    maxWsHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "최대 풍속 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgWs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 풍속(m/s)"),
                    hr24SumRws = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "풍정합(100m)"),
                    maxWd = table.Column<short>(type: "SMALLINT(4)", nullable: true, comment: "최다 풍향(16방위)"),
                    avgTd = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 이슬점 온도(c)"),
                    minRhm = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최대 상대습도(%)"),
                    minRhmHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "평균 상대습도 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgRhm = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 상대습도(%)"),
                    avgPv = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 중기압(hPa)"),
                    avgPa = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 현지기압(hPa)"),
                    maxPs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최고 해면기압"),
                    maxPsHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "최고 해면기압 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    minPs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최저 해면기압"),
                    minPsHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "최저 해면기압 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avgPs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 해면기압(hPa)"),
                    ssDur = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "가조시간(hr)"),
                    sumSsHr = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "합계 일조 시간(hr)"),
                    hr1MaxIcsrHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "1시간 최다 일사시각(MJ/m2)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hr1MaxIcsr = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "1시간 최대 일사량(MJ/m2)"),
                    sumGsr = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "합계 일사량(MJ/m2)"),
                    ddMefs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "일 최심신적설(cm)"),
                    ddMefsHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "일 최심신적설 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ddMes = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "일 최심적설(cm)"),
                    ddMesHrmt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true, comment: "일 최심적설 시각")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumDpthFhsc = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "합계 3시간 신적설(cm)"),
                    avgTca = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 전운량(10분위)"),
                    avgLmac = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 중하층운량(10분위)"),
                    avgTs = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 지면온도(c)"),
                    minTg = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "최저 초상온도(c)"),
                    avgCm5Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 5cm 지중온도(c)"),
                    avgCm10Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 10cm 지중온도(c)"),
                    avgCm20Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 20cm 지중온도(c)"),
                    avgCm30Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 30cm 지중온도(c)"),
                    avgM05Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 0.5m 지중온도(c)"),
                    avgM10Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 1.0m 지중온도(c)"),
                    avgM15Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 1.5m 지중온도(c)"),
                    avgM30Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 3.0m 지중온도(c)"),
                    avgM50Te = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "평균 5.0m 지중온도(c)"),
                    sumLrgEv = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "합계 대형증발량(mm)"),
                    sumSmlEv = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "합계 소형증발량(mm)"),
                    n99Rn = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "9-9 강수(mm)"),
                    iscs = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "일기현상")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sumFogDur = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true, comment: "안개 계속 시간(hr)"),
                    dt = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "변환시간")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRsvDailyData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRsvDailyData_FacCode_CheckDate",
                table: "JoinRsvDailyData",
                columns: new[] { "FacCode", "CheckDate" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoinRsvDailyData");
        }
    }
}
