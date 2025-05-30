using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class User : IdentityUser
{
    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();
    
    public virtual ICollection<DeckSession> DeckSessions { get; set; } = new List<DeckSession>();
}