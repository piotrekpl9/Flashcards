namespace Flashcards.Models;

public class PublicFlashcard
{
    public int Id { get; set; }

    public string? Front { get; set; } = "";

    public string? Back { get; set; } = "";

    public virtual PublicDeck? PublicDeck { get; set; }
    public int PublicDeckId { get; set; }
}