using System.Security.Claims;
using AutoMapper;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        var flashcards = await _flashcardService.GetAll();
        var filtered = flashcards.Where(f => f.DeckId == deckId.Value).ToList();
        var dtos = filtered.Select(f => _mapper.Map<FlashcardDto>(f)).ToList();
        return View("Index", dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlashcard(int id)
    {
        var flashcard = await _flashcardService.GetById(id);
        if (flashcard == null)
            return NotFound();

        var dto = _mapper.Map<FlashcardDto>(flashcard);
        return View("Show", dto);
    }

    [HttpGet("new")]
    public IActionResult New()
    {
        var decks = _context.Decks
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
            .ToList();

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

        var flashcard = await _flashcardService.Create(inputFlashcardDto, GetUserId());
        if (flashcard == null)
            return BadRequest();

        return RedirectToAction("GetFlashcard", new { deckId = flashcard.DeckId });    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var flashcard = await _flashcardService.GetById(id);
        if (flashcard == null)
            return NotFound();

        var dto = _mapper.Map<CreateFlashcardDto>(flashcard);
        ViewBag.FlashcardId = id;
        return View(dto);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(int id, CreateFlashcardDto inputFlashcardDto)
    {
        var result = await _flashcardService.Update(id, inputFlashcardDto, GetUserId());
        if (!result)
            return BadRequest();

        return RedirectToAction("GetFlashcard", new { id });
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> DeleteFlashcard(int id)
    {
        var result = await _flashcardService.Delete(id);
        if (!result)
            return BadRequest();

        return RedirectToAction("GetFlashcard");
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}