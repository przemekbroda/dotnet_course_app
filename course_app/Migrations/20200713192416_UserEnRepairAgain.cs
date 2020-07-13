using Microsoft.EntityFrameworkCore.Migrations;

namespace course_app.Migrations
{
    public partial class UserEnRepairAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interest",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interests",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "Interest",
                table: "users",
                type: "text",
                nullable: true);
        }
    }
}
