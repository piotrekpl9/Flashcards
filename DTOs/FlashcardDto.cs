namespace Flashcards.DTOs;

public class FlashcardDto
{
    public int Id { get; set; }
    public string Front { get; set; } = "";

    public string Back { get; set; } = "";

    public string UserId { get; set; } = "";
    public int DeckId { get; set; }

}