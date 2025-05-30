namespace Flashcards.DTOs;

public class DeckSessionDto
{
    public int Id { get; set; }
    
    public int? SessionLimit { get; set; }
    
    public int SessionLength { get; set; }
    
    public virtual IEnumerable<FlashcardQueueDto> FlashcardQueues { get; set; } = new List<FlashcardQueueDto>();
    public virtual FlashcardQueueDto NextFlashcardQueue { get; set; } = new FlashcardQueueDto();
}