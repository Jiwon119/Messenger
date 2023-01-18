using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "WMSDB",
                table: "SEO_FACILITY",
                type: "VARCHAR2(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cam_List",
                schema: "WMSDB",
                table: "SEO_FACILITY",
                type: "VARCHAR2(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "WMSDB",
                table: "SEO_FACILITY");

            migrationBuilder.DropColumn(
                name: "Cam_List",
                schema: "WMSDB",
                table: "SEO_FACILITY");
        }
    }
}
