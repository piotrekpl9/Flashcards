using Flashcards.Data;
using Flashcards.Models;

namespace Flashcards.Services;

public class FlashcardsQueueService
{
    private readonly ApplicationDbContext _context;
    
    public FlashcardsQueueService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task CalculatePosition(int id, int quality)
    {
        var flashcardsQueue = await _context.FlashcardsQueues.FindAsync(id);
        if (flashcardsQueue == null)
            return;

        CalculateInterval(flashcardsQueue, quality);
    }
    
    private async Task CalculateInterval(FlashcardsQueue flashcardsQueue, int quality)
    {
        var easeFactor = CalculateEaseFactor(flashcardsQueue.EaseFactor, quality);
        flashcardsQueue.EaseFactor = easeFactor;

        if (quality < 3)
        {
            flashcardsQueue.Repetitions = 0;
            flashcardsQueue.CardInterval = 1;
        }
        else
        {
            switch (flashcardsQueue.Repetitions)
            {
                case 0:
                    flashcardsQueue.CardInterval = 1;
                    break;
                case 1:
                    flashcardsQueue.CardInterval = 6;
                    break;
                default:
                    flashcardsQueue.CardInterval = (int)(flashcardsQueue.CardInterval * easeFactor);
                    break;
            }
            flashcardsQueue.Repetitions++;
        }

        flashcardsQueue.NextRunAt = DateTime.UtcNow.AddMinutes(flashcardsQueue.CardInterval);

        await _context.SaveChangesAsync();
    }
    
    private float CalculateEaseFactor(float easeFactor, int quality)
    {
        var newEaseFactor = easeFactor + (0.1f - (5 - quality) * (0.08f + (5 - quality) * 0.02f));
        return newEaseFactor <= 1.3f ? 1.3f : newEaseFactor;
    }
}