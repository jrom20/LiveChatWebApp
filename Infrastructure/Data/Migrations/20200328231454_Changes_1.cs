using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Changes_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Rooms_RoomId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_RoomId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Chats");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ChatId",
                table: "Rooms",
                column: "ChatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Chats_ChatId",
                table: "Rooms",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Chats_ChatId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ChatId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Chats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_RoomId",
                table: "Chats",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Rooms_RoomId",
                table: "Chats",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
