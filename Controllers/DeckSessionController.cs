using System.Security.Claims;
using Flashcards.Data;
using Flashcards.DTOs;
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
    
    public DeckSessionController(ApplicationDbContext context, FlashcardService flashcardService)
    {
        _context = context;
        _flashcardService = flashcardService;
    }

    [HttpGet("decks/{deckId}")]
    public async Task<IActionResult> New(int deckId)
    {
        ViewBag.DeckId = deckId;
        return View("New");
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

        var deckSessionDto = deckSessionService.MapToDeckSessionDTO(deckSession);
        
        return Ok(deckSessionDto);
    }
    
    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}