using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SEO_WATERSYSTEM",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    SENSORID = table.Column<decimal>(type: "NUMBER", nullable: true),
                    WATERLANEID = table.Column<decimal>(type: "NUMBER", nullable: true),
                    FACID = table.Column<decimal>(type: "NUMBER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_WATERSYSTEM", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SEO_WATERSYSTEM",
                schema: "WMSDB");
        }
    }
}
