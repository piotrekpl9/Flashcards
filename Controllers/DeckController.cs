using System.Security.Claims;
using AutoMapper;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Flashcards.Controllers;
[Authorize]
[Route("[controller]")]
public class DeckController : Controller
{
    private readonly DeckService _deckService;
    private readonly IMapper _mapper;

    public DeckController(IMapper mapper, DeckService deckService)
    {
        _mapper = mapper;
        _deckService = deckService;
    }


    [Route("explore")]
    public async Task<ActionResult<IEnumerable<DeckDto>>> Explore()
    {
        var decks = await _deckService.GetAllPublic();
        var deckDtos = _mapper.Map<List<DeckDto>>(decks);

        return View("Explore", deckDtos);
    }

    [Route("user/{userId}/deck")]
    public async Task<ActionResult<IEnumerable<DeckDto>>> UserDecks(string userId)
    {
        var decks = await _deckService.GetUserPublicDecks(userId);
        var deckDtos = _mapper.Map<List<DeckDto>>(decks);
        ViewBag.DeckOwnerName = decks.FirstOrDefault()?.User?.Email ?? string.Empty;
        return View("UserDecks", deckDtos);
    }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeckDto>>> GetDeck([FromQuery] string? name)
        {
            var decks = await _deckService.GetAll(GetUserId(), name);
            var deckDtos = _mapper.Map<List<DeckDto>>(decks);
            
            @ViewBag.UserId = GetUserId();
            return View("Index", deckDtos);
        }
        
        [HttpGet("new")]
        public IActionResult New()
        {
            return View(new CreateDeckDto());
        }
        
        [HttpPost("new")]
        public async Task<IActionResult> New(CreateDeckDto inputDeckDto)
        {
            if (!ModelState.IsValid)
            {
                return View(inputDeckDto);
            }

            var deck = await _deckService.Create(inputDeckDto, GetUserId());
            if (deck == null)
            {
                return BadRequest();
            }

            return RedirectToAction("GetDeck");
        }
        
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var deck = await _deckService.GetById(id);
            if (deck == null || deck.UserId != GetUserId())
                return NotFound();

            var dto = _mapper.Map<CreateDeckDto>(deck);
            ViewBag.DeckId = id;
            return View(dto);
        }
        
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, CreateDeckDto inputDeckDto)
        {
            var result = await _deckService.Update(id, inputDeckDto);
            if (!result)
                return BadRequest();

            return RedirectToAction("GetDeck");
        }

        [HttpPost]
        public async Task<ActionResult<DeckDto>> PostDeck(CreateDeckDto inputDeckDto)
        {
            var deck = await _deckService.Create(inputDeckDto, GetUserId());
            var dto = _mapper.Map<DeckDto>(deck);
            return CreatedAtAction("GetDeck", new { id = deck.Id }, dto);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteDeck(int id)
        {
            var deck = await _deckService.GetById(id);
            if (deck == null || deck.UserId != GetUserId())
            {
                return NotFound();
            }
            var result = await _deckService.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            return RedirectToAction("GetDeck");
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
}