using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SEO_Facility_Event_Log",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    FACILITYID = table.Column<decimal>(type: "NUMBER", nullable: false),
                    LOGLEVEL = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    LOGDATETIME = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: false),
                    LOGGER = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    MESSAGE = table.Column<string>(type: "CLOB", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_Facility_Event_Log", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "SEO_Facility_Event_INDEX",
                schema: "WMSDB",
                table: "SEO_Facility_Event_Log",
                columns: new[] { "FACILITYID", "LOGGER", "LOGDATETIME", "LOGLEVEL" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SEO_Facility_Event_Log",
                schema: "WMSDB");
        }
    }
}
