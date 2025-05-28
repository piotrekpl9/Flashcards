using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class Deck
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int? SessionLimit { get; set; }

    public string UserId { get; set; }
    public virtual User? User { get; set; }
    
    public string? DeckType { get; set; }
    public int? PublicDeckId { get; set; }

    public PublicDeck? PublicDeck { get; set; }
    public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

}