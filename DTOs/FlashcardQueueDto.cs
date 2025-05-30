namespace Flashcards.DTOs;

public class FlashcardQueueDto
{
    public int Id { get; set; }
    
    public int Position { get; set; }
    
    public int Repetitions { get; set; }
    
    public DateTime? NextRunAt { get; set; }
    
    public int CardInterval { get; set; }
    
    public float EaseFactor { get; set; }
    
    public FlashcardDto? Flashcard { get; set; }
}