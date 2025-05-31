using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Flashcards.Models;

public class Deck
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa wymagana")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Limit pytań wymagany")]
    [Range(1, int.MaxValue, ErrorMessage = "Musi być większe niż 0.")]
    public int? SessionLimit { get; set; }

    public string UserId { get; set; }
    public virtual User? User { get; set; }
    public DeckStatus Status { get; set; }
    public string? DeckType { get; set; }
    public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

}