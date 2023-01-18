using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetServerDemo.Migrations
{
    public partial class UserImgTableAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Profiles",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profiles", x => x.UserID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Profiles");
        }
    }
}
