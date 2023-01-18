using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetServerDemo.Migrations
{
    public partial class UserChatTableUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Chat",
                table: "User_Chat");

            migrationBuilder.RenameTable(
                name: "User_Chat",
                newName: "User_Chats");

            migrationBuilder.AlterColumn<int>(
                name: "RoomID",
                table: "User_Chats",
                type: "int",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Chats",
                table: "User_Chats",
                columns: new[] { "UserID", "RoomID", "ChatID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Chats",
                table: "User_Chats");

            migrationBuilder.RenameTable(
                name: "User_Chats",
                newName: "User_Chat");

            migrationBuilder.AlterColumn<sbyte>(
                name: "RoomID",
                table: "User_Chat",
                type: "tinyint(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Chat",
                table: "User_Chat",
                columns: new[] { "UserID", "RoomID", "ChatID" });
        }
    }
}
