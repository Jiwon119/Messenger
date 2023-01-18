using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SEO_Facility_Event_Log",
                schema: "WMSDB",
                table: "SEO_Facility_Event_Log");

            migrationBuilder.RenameTable(
                name: "SEO_Facility_Event_Log",
                schema: "WMSDB",
                newName: "SEO_FACILITY_EVENT_LOG",
                newSchema: "WMSDB");

            migrationBuilder.RenameColumn(
                name: "FACILITYID",
                schema: "WMSDB",
                table: "SEO_FACILITY_EVENT_LOG",
                newName: "FACILITY_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SEO_FACILITY_EVENT_LOG",
                schema: "WMSDB",
                table: "SEO_FACILITY_EVENT_LOG",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SEO_FACILITY_EVENT_LOG",
                schema: "WMSDB",
                table: "SEO_FACILITY_EVENT_LOG");

            migrationBuilder.RenameTable(
                name: "SEO_FACILITY_EVENT_LOG",
                schema: "WMSDB",
                newName: "SEO_Facility_Event_Log",
                newSchema: "WMSDB");

            migrationBuilder.RenameColumn(
                name: "FACILITY_ID",
                schema: "WMSDB",
                table: "SEO_Facility_Event_Log",
                newName: "FACILITYID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SEO_Facility_Event_Log",
                schema: "WMSDB",
                table: "SEO_Facility_Event_Log",
                column: "ID");
        }
    }
}
