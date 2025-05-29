using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class PublicDeck
{
    
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public DateTime? ConfirmedAt { get; set; }
}