using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicDBLib.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RsvCodeList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacCode = table.Column<string>(type: "varchar(255)", nullable: false, comment: "표준 코드")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FacName = table.Column<string>(type: "longtext", nullable: true, comment: "시설명")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "longtext", nullable: true, comment: "소재지")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YearOfStart = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "착공년도"),
                    YearOfConstruction = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "준공년도"),
                    Classification = table.Column<int>(type: "int", nullable: true, comment: "종별"),
                    Division = table.Column<string>(type: "longtext", nullable: true, comment: "구분")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WatershedArea = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "유역면적"),
                    FloodArea = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "홍수면적"),
                    FullArea = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "만수면적"),
                    BeneficiaryArea = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "수혜면적"),
                    OneStepFrequency = table.Column<int>(type: "int", nullable: true, comment: "한발빈도"),
                    FloodFrequency = table.Column<int>(type: "int", nullable: true, comment: "홍수빈도"),
                    BodyFormat = table.Column<string>(type: "longtext", nullable: true, comment: "제체형식")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BodyVolume = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "제체제적"),
                    BodyLength = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "제체길이"),
                    BodyHeight = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "제체높이"),
                    TotalStorage = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "총저수량"),
                    EffectiveStorage = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "유효저수량"),
                    Gunnery = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "사수량"),
                    WaterIntake = table.Column<string>(type: "longtext", nullable: true, comment: "취수형식")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FloodLevel = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "홍수위"),
                    WaterLevel = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "만수위"),
                    Janitor = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "사수위")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RsvCodeList", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RsvDailyData",
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
                    Rate = table.Column<decimal>(type: "decimal(20,2)", precision: 20, scale: 2, nullable: true, comment: "저수율")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RsvDailyData", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DamCodeList_DamCode",
                table: "DamCodeList",
                column: "DamCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ASOSCodeList_WPCode",
                table: "ASOSCodeList",
                column: "WPCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RsvCodeList_FacCode",
                table: "RsvCodeList",
                column: "FacCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RsvDailyData_FacCode_CheckDate",
                table: "RsvDailyData",
                columns: new[] { "FacCode", "CheckDate" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RsvCodeList");

            migrationBuilder.DropTable(
                name: "RsvDailyData");

            migrationBuilder.DropIndex(
                name: "IX_DamCodeList_DamCode",
                table: "DamCodeList");

            migrationBuilder.DropIndex(
                name: "IX_ASOSCodeList_WPCode",
                table: "ASOSCodeList");
        }
    }
}
