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
    private readonly IMapper _mapper;
    public AdminController(DeckService deckService, IMapper mapper)
    {
        _deckService = deckService;
        _mapper = mapper;
    }

    [HttpGet("pending-decks")]
    public async Task<ActionResult<IEnumerable<DeckDto>>> GetPendingDecks()
    {
        var pendingDecks = await _deckService.GetPendingDecks();
        var deckDtos = _mapper.Map<List<DeckDto>>(pendingDecks);
        return View("Index", deckDtos);
    }

    [HttpPost("decks/{id}/approve")]
    public async Task<IActionResult> ApproveDeck(int id)
    {
        var result = await _deckService.ApproveDeck(id);
        if (!result)
        {
            return BadRequest();
        }
        return Ok();
    }
    
    // POST: /admin/decks/{id}/reject
    [HttpPost("decks/{id}/reject")]
    public async Task<IActionResult> RejectDeck(int id)
    {
        var result = await _deckService.RejectDeck(id);
        if (!result)
        {
            return BadRequest();
        }
        return Ok();
    }
}