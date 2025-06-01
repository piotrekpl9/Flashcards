using AutoMapper;
using Flashcards.DTOs;
using Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Controllers;

[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly DeckService _deckService;
    private readonly FlashcardService _flashcardService;
    private readonly IMapper _mapper;
    public AdminController(DeckService deckService, IMapper mapper, FlashcardService flashcardService)
    {
        _deckService = deckService;
        _mapper = mapper;
        _flashcardService = flashcardService;
    }

    [HttpGet("pending-decks")]
    public async Task<ActionResult<IEnumerable<DeckDto>>> GetPendingDecks([FromQuery] string? name)
    {
        var pendingDecks = await _deckService.GetPendingDecks(name);
        var deckDtos = _mapper.Map<List<DeckDto>>(pendingDecks);
        return View("Index", deckDtos);
    }
    [HttpGet("pending-decks/{id:int}/flashcards")]
    public async Task<ActionResult<IEnumerable<FlashcardDto>>> GetPendingDecks(int id)
    {
        var pendingDecks = await _flashcardService.GetAllByDeckId(id);
        var deckDtos = _mapper.Map<List<FlashcardDto>>(pendingDecks);
        return View("Flashcards", deckDtos);
    }
    [HttpPost("decks/{id}/approve")]
    public async Task<IActionResult> ApproveDeck(int id)
    {
        var result = await _deckService.AcceptDeck(id);
        if (!result)
        {
            return BadRequest();
        }
        return RedirectToAction("GetPendingDecks");

    }
    
    [HttpPost("decks/{id}/reject")]
    public async Task<IActionResult> RejectDeck(int id)
    {
        var result = await _deckService.RejectDeck(id);
        if (!result)
        {
            return BadRequest();
        }
        return RedirectToAction("GetPendingDecks");

    }
}