using Flashcards.Models;

namespace Flashcards.DTOs;

public class PublicDeckDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string UserId { get; set; } = "";
    public IEnumerable<DeckDto> Decks { get; set; } = [];
    public IEnumerable<PublicFlashcardDto> PublicFlashcards { get; set; } = [];
}