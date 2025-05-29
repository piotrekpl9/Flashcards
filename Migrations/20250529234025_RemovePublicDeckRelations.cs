using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcards.Migrations
{
    /// <inheritdoc />
    public partial class RemovePublicDeckRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicDecks_AspNetUsers_UserId",
                table: "PublicDecks");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicFlashcards_PublicDecks_PublicDeckId",
                table: "PublicFlashcards");

            migrationBuilder.DropIndex(
                name: "IX_PublicFlashcards_PublicDeckId",
                table: "PublicFlashcards");

            migrationBuilder.DropIndex(
                name: "IX_PublicDecks_UserId",
                table: "PublicDecks");

            migrationBuilder.DropColumn(
                name: "PublicDeckId",
                table: "PublicFlashcards");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PublicDecks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublicDeckId",
                table: "PublicFlashcards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PublicDecks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PublicFlashcards_PublicDeckId",
                table: "PublicFlashcards",
                column: "PublicDeckId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicDecks_UserId",
                table: "PublicDecks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDecks_AspNetUsers_UserId",
                table: "PublicDecks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicFlashcards_PublicDecks_PublicDeckId",
                table: "PublicFlashcards",
                column: "PublicDeckId",
                principalTable: "PublicDecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
