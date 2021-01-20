using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineChatApp.Migrations
{
    public partial class NorNameAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedFirstName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedLastName",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedFirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedLastName",
                table: "Users");
        }
    }
}
