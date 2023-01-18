using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetServerDemo.Migrations
{
    public partial class UserImgTableAdd_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Img",
                table: "User_Profiles",
                type: "longtext",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10000)",
                oldUnicode: false,
                oldMaxLength: 10000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Img",
                table: "User_Profiles",
                type: "varchar(10000)",
                unicode: false,
                maxLength: 10000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldUnicode: false)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
