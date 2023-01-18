using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ORDER",
                schema: "WMSDB",
                table: "SEO_WATERSYSTEM",
                type: "NUMBER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ORDER",
                schema: "WMSDB",
                table: "SEO_WATERSYSTEM");
        }
    }
}
