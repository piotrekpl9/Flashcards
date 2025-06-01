using Flashcards.Models;

public static class DeckStatusExtensions
{
    public static string ToPolish(this DeckStatus status)
    {
        return status switch
        {
            DeckStatus.Pending => "Oczekujący",
            DeckStatus.Accepted => "Zaakceptowany",
            DeckStatus.Rejected => "Odrzucony",
            _ => "Nieznany"
        };
    }
}