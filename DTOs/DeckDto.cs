namespace Flashcards.DTOs;

public class DeckDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public int? SessionLimit { get; set; }

    public string UserId { get; set; }
    
    public string? DeckType { get; set; }

    public virtual IEnumerable<FlashcardDto> Flashcards { get; set; } = new List<FlashcardDto>();
}