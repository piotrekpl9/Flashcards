namespace Flashcards.DTOs;

public class PublicFlashcardDto
{
    public int Id { get; set; }
    public string Front { get; set; } = "";
    public string Back { get; set; }= "";
    public int PublicDeckId { get; set; }
}