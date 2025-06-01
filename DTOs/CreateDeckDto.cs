using System.ComponentModel.DataAnnotations;

namespace Flashcards.DTOs;

public class CreateDeckDto
{
    [Display(Name = "Nazwa")]
    [Required(ErrorMessage = "Nazwa wymagana")]
    public string Name { get; set; } 

    [Display(Name = "Długość sesji")]
    [Required(ErrorMessage = "Limit pytań wymagany")]
    [Range(1, int.MaxValue, ErrorMessage = "Musi być większe niż 0.")]
    public int? SessionLimit { get; set; }
    
    public string? DeckType { get; set; }
    
}