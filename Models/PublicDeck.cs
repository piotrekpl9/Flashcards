using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class PublicDeck
{
    
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public DateTime? ConfirmedAt { get; set; }

    public virtual User? User { get; set; }
    public string UserId { get; set; }

    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();

    public virtual ICollection<PublicFlashcard> PublicFlashcards { get; set; } = new List<PublicFlashcard>();
}