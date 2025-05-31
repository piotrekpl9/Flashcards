using System.Security.Claims;
using AutoMapper;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Controllers;

[Authorize]
[Route("[controller]")]
public class FlashcardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly FlashcardService _flashcardService;
    private readonly IMapper _mapper;

    public FlashcardController(ApplicationDbContext context, IMapper mapper, FlashcardService flashcardService)
    {
        _context = context;
        _mapper = mapper;
        _flashcardService = flashcardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFlashcard([FromQuery] int? deckId)
    {
        if (deckId == null)
            return BadRequest("deckId is required.");

        var flashcards = await _flashcardService.GetAll(GetUserId());
        var filtered = flashcards.Where(f => f.DeckId == deckId.Value).ToList();
        var dtos = filtered.Select(f => _mapper.Map<FlashcardDto>(f)).ToList();
        return View("Index", dtos);
    }

    [HttpGet("new")]
    public IActionResult New()
    {
        var decks = GetUserDecks();

        var model = new CreateFlashcardDto
        {
            Decks = decks
        };

        return View(model);
    }

    [HttpPost("new")]
    public async Task<IActionResult> New(CreateFlashcardDto inputFlashcardDto)
    {
        if (!ModelState.IsValid)
            return View(inputFlashcardDto);
        
        var deck = await _context.Decks
            .FirstOrDefaultAsync(d => d.Id == inputFlashcardDto.DeckId && d.UserId == GetUserId());
        
        if (deck == null)
            return NotFound();

        var flashcard = await _flashcardService.Create(inputFlashcardDto, GetUserId(), deck.Id);
        if (flashcard == null)
            return BadRequest();

        return RedirectToAction("GetFlashcard", new { flashcard.DeckId });    
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var flashcard = await _context.Flashcards
            .Include(f => f.Deck)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        if (flashcard == null || flashcard.Deck == null || flashcard.Deck.UserId != GetUserId())
            return NotFound();
        
        var model = new CreateFlashcardDto
        {
            Front = flashcard.Front,
            Back = flashcard.Back
        };

        ViewBag.FlashcardId = id;
        return View(model);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(int id, CreateFlashcardDto inputFlashcardDto)
    {
        if (!ModelState.IsValid)
            return View(inputFlashcardDto);

        var updatedFlashcard = await _flashcardService.Update(id, inputFlashcardDto, GetUserId());
        if (updatedFlashcard == null)
            return NotFound();

        return RedirectToAction("GetFlashcard", new { deckId = updatedFlashcard.Deck.Id });
    }
    
    [HttpPost("{id}")]
    public async Task<IActionResult> DeleteFlashcard(int id)
    {
        var flashcard = await _flashcardService.GetById(id);
        var deckId = flashcard?.DeckId;
        if (flashcard == null || flashcard.UserId != GetUserId())
        {
            return NotFound();
        }
        var result = await _flashcardService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return RedirectToAction("GetFlashcard", new { deckId });
    }


    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
    
    private List<SelectListItem> GetUserDecks()
    {
        return _context.Decks
            .Where(d => d.UserId == GetUserId())
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
            .ToList();
    }
}