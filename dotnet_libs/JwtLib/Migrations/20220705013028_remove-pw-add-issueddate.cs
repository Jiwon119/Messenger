using Microsoft.EntityFrameworkCore.Migrations;

namespace JwtLib.Migrations
{
    public partial class removepwaddissueddate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "IssuedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IssuedDate",
                table: "Users",
                newName: "Password");
        }
    }
}
