using Microsoft.EntityFrameworkCore.Migrations;

namespace RTUDBLib.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is_TEMP_PW",
                schema: "WMSDB",
                table: "SEO_ACCOUNT",
                newName: "IS_TEMP_PW");

            migrationBuilder.AddColumn<decimal>(
                name: "FAC_TYPE",
                schema: "WMSDB",
                table: "SEO_MAP_AREA",
                type: "NUMBER",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SCENE_NUM",
                schema: "WMSDB",
                table: "SEO_MAP_AREA",
                type: "NUMBER",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RTU_LIST",
                schema: "WMSDB",
                table: "SEO_FACILITY",
                type: "VARCHAR2(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SEO_FACILITY_SCENE_INFO",
                schema: "WMSDB",
                columns: table => new
                {
                    ID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "1, 1"),
                    FACILITY_ID = table.Column<decimal>(type: "NUMBER", nullable: false, comment: "시설 아이디"),
                    RTU_ID = table.Column<decimal>(type: "NUMBER", nullable: false, comment: "연결된 RTU 아이디"),
                    UNIT_ID = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false, comment: "유닛 아이디"),
                    SCENE_NUM = table.Column<decimal>(type: "NUMBER", nullable: false, comment: "뷰 번호"),
                    RENDER_ORDER = table.Column<decimal>(type: "NUMBER", nullable: false, comment: "렌더링 순서")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEO_FACILITY_SCENE_INFO", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SEO_FACILITY_SCENE_INFO",
                schema: "WMSDB");

            migrationBuilder.DropColumn(
                name: "FAC_TYPE",
                schema: "WMSDB",
                table: "SEO_MAP_AREA");

            migrationBuilder.DropColumn(
                name: "SCENE_NUM",
                schema: "WMSDB",
                table: "SEO_MAP_AREA");

            migrationBuilder.DropColumn(
                name: "RTU_LIST",
                schema: "WMSDB",
                table: "SEO_FACILITY");

            migrationBuilder.RenameColumn(
                name: "IS_TEMP_PW",
                schema: "WMSDB",
                table: "SEO_ACCOUNT",
                newName: "Is_TEMP_PW");
        }
    }
}
