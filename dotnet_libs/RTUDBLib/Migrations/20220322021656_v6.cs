using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "POS1",
                schema: "WMSDB",
                table: "SEO_FACILITY",
                type: "VARCHAR2(45)",
                unicode: false,
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POS2",
                schema: "WMSDB",
                table: "SEO_FACILITY",
                type: "VARCHAR2(45)",
                unicode: false,
                maxLength: 45,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "POS1",
                schema: "WMSDB",
                table: "SEO_FACILITY");

            migrationBuilder.DropColumn(
                name: "POS2",
                schema: "WMSDB",
                table: "SEO_FACILITY");
        }
    }
}
