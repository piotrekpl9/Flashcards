using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcards.Migrations
{
    /// <inheritdoc />
    public partial class RemovePublicDeckRelationFromDeck2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_PublicDecks_PublicDeckId",
                table: "Decks");

            migrationBuilder.DropIndex(
                name: "IX_Decks_PublicDeckId",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "PublicDeckId",
                table: "Decks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublicDeckId",
                table: "Decks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_PublicDeckId",
                table: "Decks",
                column: "PublicDeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_PublicDecks_PublicDeckId",
                table: "Decks",
                column: "PublicDeckId",
                principalTable: "PublicDecks",
                principalColumn: "Id");
        }
    }
}
