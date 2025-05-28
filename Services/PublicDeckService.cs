using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Services;

public class PublicDeckService
{
    
    private readonly ApplicationDbContext _context;

    public PublicDeckService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PublicDeck>> GetAll()
    {
        return [];
    }
    
    public async Task<PublicDeck?> GetById(int id)
    {
        return await _context.PublicDecks.Where(deck => deck.Id == id).Include(deck => deck.PublicFlashcards).FirstOrDefaultAsync();
    }

    public async Task<PublicDeck> Create(CreatePublicDeckDto inputDeckDto, string userId)
    {
      
        return new PublicDeck();
    }
    
    public async Task<bool> Update(int id, CreatePublicDeckDto inputDeckDto, string userId)
    {
        if (!PublicDeckExists(id))
        {
            return false;
        }

        //
        //
        // _context.Entry(deck).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> Delete(int id)
    { 
        if (!PublicDeckExists(id))
        {
            return false;
        }
        var deck = await _context.PublicDecks.FindAsync(id);
        if (deck == null)
        {
            return false;
        }

        _context.PublicDecks.Remove(deck);
        await _context.SaveChangesAsync();
        return true;
    }
    private bool PublicDeckExists(int id)
    {
        return _context.PublicDecks.Any(e => e.Id == id);
    }
}