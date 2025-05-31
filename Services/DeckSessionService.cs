using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Services;

public class DeckSessionService
{
    private readonly ApplicationDbContext _context;
    private readonly FlashcardService _flashcardService;
    
    public DeckSessionService(ApplicationDbContext context, FlashcardService flashcardService)
    {
        _context = context;
        _flashcardService = flashcardService;
    }
    
    public async Task<DeckSession?> CreateDeckSession(User currentUser, int deckId)
    {
        var deck = currentUser.Decks.FirstOrDefault(d => d.Id == deckId);
        if (deck == null)
        {
            deck = await _context.Decks
                .Where(d => d.Id == deckId && d.Status == DeckStatus.Accepted)
                .Include(d => d.Flashcards)
                .FirstOrDefaultAsync();
            if (deck == null)
                return null;
        }

        var deckSession = new DeckSession
        {
            Deck = deck,
            DeckId = deck.Id,
            User = currentUser,
            UserId = currentUser.Id,
            SessionLimit = deck.SessionLimit,
            SessionLength = 0
        };

        _context.DeckSessions.Add(deckSession);
        await _context.SaveChangesAsync();

        foreach (var flashcard in deck.Flashcards)
        {
            CreateFlashcardsQueue(deckSession, flashcard);
        }

        await _context.SaveChangesAsync();

        return deckSession;
    }
    
    public DeckSessionDto MapToDeckSessionDTO(DeckSession deckSession)
    {
        return new DeckSessionDto
        {
            Id = deckSession.Id,
            SessionLimit = deckSession.SessionLimit,
            SessionLength = deckSession.SessionLength,
            FlashcardQueues = deckSession.FlashcardsQueues
                .Select(fq => MapToFlashcardsQueueDto(fq))
                .ToList(),
            NextFlashcardQueue = GetNextFlashcardsQueue(deckSession)
        };
    }
    
    public async Task IncrementSessionLength(int deckSessionId)
    {
        var deckSession = await _context.DeckSessions.FindAsync(deckSessionId);
        if (deckSession == null)
            throw new InvalidOperationException("DeckSession not found");

        deckSession.SessionLength++;
        await _context.SaveChangesAsync();
    }
    
    private FlashcardQueueDto MapToFlashcardsQueueDto(FlashcardsQueue flashcardsQueue)
    {
        return new FlashcardQueueDto
        {
            Id = flashcardsQueue.Id,
            Position = flashcardsQueue.Position,
            Repetitions = flashcardsQueue.Repetitions,
            NextRunAt = flashcardsQueue.NextRunAt,
            CardInterval = flashcardsQueue.CardInterval,
            EaseFactor = flashcardsQueue.EaseFactor,
            Flashcard = _flashcardService.MapToFlashcardDto(flashcardsQueue.Flashcard!)
        };
    }
    
    private void CreateFlashcardsQueue(DeckSession deckSession, Flashcard flashcard)
    {
        var queue = new FlashcardsQueue
        {
            DeckSession = deckSession,
            DeckSessionId = deckSession.Id,
            Flashcard = flashcard,
            FlashcardId = flashcard.Id,
            NextRunAt = DateTime.UtcNow,
        };
        _context.FlashcardsQueues.Add(queue);
    }
    
    private FlashcardQueueDto? GetNextFlashcardsQueue(DeckSession deckSession)
    {
        var nextFlashcardsQueue = deckSession.FlashcardsQueues
            .Where(fq => fq.NextRunAt != null)
            .OrderBy(fq => fq.NextRunAt)
            .FirstOrDefault();

        if (nextFlashcardsQueue == null)
            return null;

        return MapToFlashcardsQueueDto(nextFlashcardsQueue);
    }
}