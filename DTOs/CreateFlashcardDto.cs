namespace Flashcards.DTOs;

public class CreateFlashcardDto
{
    public string Front { get; set; } = "";

    public string Back { get; set; } = "";

    public int DeckId { get; set; }
}