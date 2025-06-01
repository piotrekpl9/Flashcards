using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Flashcards.DTOs;

public class CreateFlashcardDto
{
    [Display(Name = "Pytanie")]
    [Required(ErrorMessage = "Pytanie wymagane")]
    public string Front { get; set; }
    
    [Display(Name = "Odpowiedź")]
    [Required(ErrorMessage = "Odpowiedź wymagana")]
    public string Back { get; set; }
    
    public int DeckId { get; set; }
    
    public IEnumerable<SelectListItem> Decks { get; set; } = new List<SelectListItem>();
}