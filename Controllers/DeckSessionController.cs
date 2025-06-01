using System.Security.Claims;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Controllers;

[Authorize]
[Route("[controller]")]
public class DeckSessionController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly FlashcardService _flashcardService;
    private readonly FlashcardsQueueService _flashcardsQueueService;
    private readonly DeckSessionService _deckSessionService;
    
    public DeckSessionController(
        ApplicationDbContext context, 
        FlashcardService flashcardService,
        FlashcardsQueueService flashcardsQueueService,
        DeckSessionService deckSessionService
        )
    {
        _context = context;
        _flashcardService = flashcardService;
        _flashcardsQueueService = flashcardsQueueService;
        _deckSessionService = deckSessionService;
    }

    [HttpPost("decks/{deckId}")]
    public async Task<IActionResult> Create(int deckId)
    {
        var userId = GetUserId();
        var currentUser = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Decks)
            .ThenInclude(d => d.Flashcards)
            .FirstOrDefaultAsync();

        if (currentUser == null)
            return Unauthorized();

        var deckSessionService = new DeckSessionService(_context, _flashcardService);
        var deckSession = await deckSessionService.CreateDeckSession(currentUser, deckId);

        if (deckSession == null)
            return NotFound();
        
        return RedirectToAction("Play", new { deckSessionId = deckSession.Id });
    }
    
    [HttpGet("deck_sessions/{deckSessionId}/play")]
    public async Task<IActionResult> Play(int deckSessionId)
    {
        var userId = GetUserId();
        var currentUser = await _context.Users
            .Include(u => u.DeckSessions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (currentUser == null)
            return Unauthorized();

        var deckSession = await _context.DeckSessions
            .Include(ds => ds.FlashcardsQueues)
            .ThenInclude(fq => fq.Flashcard)
            .FirstOrDefaultAsync(ds => ds.Id == deckSessionId && ds.UserId == userId);
        if (deckSession == null)
            return NotFound("Session not found");

        if (deckSession.SessionLimit != null && deckSession.SessionLength >= deckSession.SessionLimit)
        {
            TempData["FlashMessage"] = "Gra zakończona!";
            return RedirectToAction("GetDeck", "Deck");
        }

        var deckSessionService = new DeckSessionService(_context, _flashcardService);
        var dto = deckSessionService.MapToDeckSessionDTO(deckSession);
        
        return View("Play", dto);
    }
    
    [HttpPost("deck_sessions/{deckSessionId}/answer")]
    public async Task<IActionResult> Update(int deckSessionId, AnswerQuestionDto answerQuestionDto)    {
        var userId = GetUserId();
        var currentUser = await _context.Users
            .Include(u => u.DeckSessions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (currentUser == null)
            return Unauthorized();

        var deckSession = currentUser.DeckSessions.FirstOrDefault(ds => ds.Id == deckSessionId);
        if (deckSession == null)
            return NotFound("Session not found");

        // Call your services (implement these as needed)
        await _flashcardsQueueService.CalculatePosition(answerQuestionDto.FlashcardsQueueId, answerQuestionDto.Quality);
        await _deckSessionService.IncrementSessionLength(deckSession.Id);

        return RedirectToAction("Play", new { deckSessionId = deckSession.Id });
    }
    
    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}