using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetServerDemo.Migrations
{
    public partial class UserRoomTableModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_Rooms",
                table: "user_Rooms");

            migrationBuilder.RenameTable(
                name: "user_Rooms",
                newName: "User_Rooms");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "User_Rooms",
                type: "datetime(6)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Rooms",
                table: "User_Rooms",
                columns: new[] { "UserID", "RoomID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Rooms",
                table: "User_Rooms");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "User_Rooms");

            migrationBuilder.RenameTable(
                name: "User_Rooms",
                newName: "user_Rooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_Rooms",
                table: "user_Rooms",
                columns: new[] { "UserID", "RoomID" });
        }
    }
}
