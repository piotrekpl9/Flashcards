using Microsoft.AspNetCore.Mvc.Rendering;

namespace Flashcards.DTOs;

public class CreateDeckSessionDto
{
    public int? SessionLimit { get; set; }
    
    public int SessionLength { get; set; }
    
    public virtual IEnumerable<FlashcardQueueDto> FlashcardQueues { get; set; } = new List<FlashcardQueueDto>();
    public virtual FlashcardQueueDto NextFlashcardQueue { get; set; } = new FlashcardQueueDto();
}