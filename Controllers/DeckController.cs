using System.Security.Claims;
using AutoMapper;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Controllers;
// [Authorize]
[Route("[controller]")]
public class DeckController : Controller
{
    private readonly DeckService _deckService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeckController(ApplicationDbContext context, IMapper mapper, DeckService deckService)
    {
        _context = context;
        _mapper = mapper;
        _deckService = deckService;
    }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeckDto>>> GetDeck()
        {
            var decks = await _deckService.GetAll();
            var deckDtos = _mapper.Map<List<DeckDto>>(decks);
            return View("Index", deckDtos);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Deck>> GetDeck(int id)
        {
            var deck = await _deckService.GetById(id);

            if (deck == null)
            {
                return NotFound();
            }

            return View("Show", _mapper.Map<DeckDto>(deck));
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeck(int id, CreateDeckDto inputDeckDto)
        {
            var result = await _deckService.Update(id, inputDeckDto, GetUserId());
            if (!result)
            {
              return BadRequest();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<DeckDto>> PostDeck(CreateDeckDto inputDeckDto)
        {
            var deck = await _deckService.Create(inputDeckDto, GetUserId());
            var dto = _mapper.Map<DeckDto>(deck);
            return CreatedAtAction("GetDeck", new { id = deck.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeck(int id)
        {
            var result = await _deckService.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
}