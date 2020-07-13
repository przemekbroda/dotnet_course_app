using Microsoft.EntityFrameworkCore.Migrations;

namespace course_app.Migrations
{
    public partial class UserEnRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interes",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "Interest",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interest",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "Interes",
                table: "users",
                type: "text",
                nullable: true);
        }
    }
}
