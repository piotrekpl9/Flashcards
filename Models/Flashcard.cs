using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class Flashcard
{
    public int Id { get; set; }
    
    public string Front { get; set; } = "";

    public string Back { get; set; } = "";

    public virtual User User { get; set; }
    public string UserId { get; set; }
    
    public virtual Deck Deck { get; set; }
    public int DeckId { get; set; }

    public virtual ICollection<FlashcardsQueue> FlashcardsQueues { get; set; } = new HashSet<FlashcardsQueue>();
}