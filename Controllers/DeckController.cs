using System.Security.Claims;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DeckController : ControllerBase
{
      private readonly ApplicationDbContext _context;

        public DeckController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            var username = User.Identity?.Name;
            return Ok($"Hello, {username}!");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deck>>> GetDeck()
        {
            return await _context.Decks
                .Include(e => e.User)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Deck>> GetDeck(int id)
        {
            var entry = await _context.Decks
                .Include(e => e.User)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (entry == null)
            {
                return NotFound();
            }

            return  entry;
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeck(int id, CreateDeck inputDeck)
        {
            var deck = new Deck()
            {
                Id = id,
                Name = inputDeck.Name,
                UserId = GetUserId(),
                SessionLimit = inputDeck.SessionLimit,
                DeckType = inputDeck.DeckType,
            };

            _context.Entry(deck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Deck>> PostDeck(CreateDeck inputDeck)
        {
            var deck = new Deck
            {
                Name = inputDeck.Name,
                UserId = GetUserId(),
                SessionLimit = inputDeck.SessionLimit,
                DeckType = inputDeck.DeckType,
            };

            _context.Decks.Add(deck);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeck", new { id = deck.Id }, deck);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeck(int id)
        {
            var entry = await _context.Decks.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.Decks.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeckExists(int id)
        {
            return _context.Decks.Any(e => e.Id == id);
        }
    
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
}