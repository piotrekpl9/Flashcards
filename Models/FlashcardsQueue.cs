namespace Flashcards.Models;
public class FlashcardsQueue
{
    public int Id { get; set; }

    public int Position { get; set; } = 0;

    public int Repetitions { get; set; } = 0;

    public DateTime? NextRunAt { get; set; }

    public int CardInterval { get; set; } = 1;

    public float EaseFactor { get; set; } = 2.5f;

    public virtual Flashcard? Flashcard { get; set; }
    public int? FlashcardId { get; set; }

    public virtual DeckSession? DeckSession { get; set; }
    public int? DeckSessionId { get; set; }
}