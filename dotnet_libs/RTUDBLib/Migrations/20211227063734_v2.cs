using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ISNEWUNIT",
                schema: "WMSDB",
                table: "SEO_UNIT",
                type: "NUMBER",
                nullable: false,
                defaultValueSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISNEWUNIT",
                schema: "WMSDB",
                table: "SEO_UNIT");
        }
    }
}
