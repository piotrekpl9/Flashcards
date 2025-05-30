using Microsoft.AspNetCore.Mvc.Rendering;

namespace Flashcards.DTOs;

public class CreateFlashcardDto
{
    public string Front { get; set; } = "";

    public string Back { get; set; } = "";

    public int DeckId { get; set; }
    
    public IEnumerable<SelectListItem> Decks { get; set; } = new List<SelectListItem>();
}