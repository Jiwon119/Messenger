using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetServerDemo.Migrations
{
    public partial class UserImgTableAdd_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Img",
                table: "User_Profiles",
                type: "varchar(10000)",
                unicode: false,
                maxLength: 10000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3000)",
                oldUnicode: false,
                oldMaxLength: 3000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Img",
                table: "User_Profiles",
                type: "varchar(3000)",
                unicode: false,
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10000)",
                oldUnicode: false,
                oldMaxLength: 10000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
