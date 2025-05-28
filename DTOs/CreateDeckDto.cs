using System.ComponentModel.DataAnnotations;

namespace Flashcards.DTOs;

public class CreateDeckDto
{
    [Required]
    public string Name { get; set; } 

    public int? SessionLimit { get; set; }
    
    public string? DeckType { get; set; }
    
}