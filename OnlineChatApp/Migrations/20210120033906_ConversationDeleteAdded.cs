using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineChatApp.Migrations
{
    public partial class ConversationDeleteAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationDeletes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoomId = table.Column<long>(nullable: false),
                    ConversationId = table.Column<long>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: false),
                    ForAll = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationDeletes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationDeletes");
        }
    }
}
