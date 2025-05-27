using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class DeckSession
{
    public int Id { get; set; }

    public virtual Deck? Deck { get; set; }
    public int? DeckId { get; set; }

    public virtual User? User { get; set; }
    public string UserId { get; set; }

    public int? SessionLimit { get; set; }

    public int SessionLength { get; set; } = 0;

    public virtual ICollection<FlashcardsQueue> FlashcardsQueues { get; set; } = new HashSet<FlashcardsQueue>();
}