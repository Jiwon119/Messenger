using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FAILED_COUNT",
                schema: "WMSDB",
                table: "SEO_ACCOUNT",
                type: "NUMBER",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FAILED_COUNT",
                schema: "WMSDB",
                table: "SEO_ACCOUNT");
        }
    }
}
