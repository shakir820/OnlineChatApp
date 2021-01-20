using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineChatApp.Migrations
{
    public partial class ChatRoomTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatRoomType",
                table: "ChatRooms",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatRoomType",
                table: "ChatRooms");
        }
    }
}
