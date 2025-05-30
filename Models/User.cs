using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class User : IdentityUser
{
    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();
}