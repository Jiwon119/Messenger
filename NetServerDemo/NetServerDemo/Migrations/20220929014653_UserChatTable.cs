using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetServerDemo.Migrations
{
    public partial class UserChatTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Chat",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChatID = table.Column<int>(type: "int", unicode: false, maxLength: 1, nullable: false),
                    RoomID = table.Column<bool>(type: "tinyint(20)", unicode: false, maxLength: 20, nullable: false),
                    Check = table.Column<bool>(type: "tinyint(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Chat", x => new { x.UserID, x.RoomID, x.ChatID });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Chat");
        }
    }
}
