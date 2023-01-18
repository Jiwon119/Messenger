using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Is_TEMP_PW",
                schema: "WMSDB",
                table: "SEO_ACCOUNT",
                type: "NUMBER",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_TEMP_PW",
                schema: "WMSDB",
                table: "SEO_ACCOUNT");
        }
    }
}
