using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Services;

public class DeckService
{
    private readonly ApplicationDbContext _context;

    public DeckService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Deck>> GetAll(string userId)
    {
        return await _context.Decks
            .Where(deck => deck.UserId == userId)
            .Include(e => e.User)
            .Include(deck => deck.Flashcards)
            .ToListAsync();
    }
    public async Task<IEnumerable<Deck>> GetAllPublic()
    {
        return await _context.Decks
            .Include(e => e.User)
            .Include(deck => deck.Flashcards)
            .Where(deck =>  deck.Status == DeckStatus.Accepted)
            .ToListAsync();
    }
    public async Task<IEnumerable<Deck>> GetUserPublicDecks(string userId)
    {
        return await _context.Decks
            .Include(e => e.User)
            .Include(deck => deck.Flashcards)
            .Where(deck => deck.UserId == userId && deck.Status == DeckStatus.Accepted)
            .ToListAsync();
    }
    public async Task<IEnumerable<Deck>> GetPendingDecks()
    {
        return await _context.Decks
            .Include(e => e.User)
            .Include(deck => deck.Flashcards)
            .Where(deck => deck.Status == DeckStatus.Pending)
            .ToListAsync();
    }
    public async Task<bool> AcceptDeck(int id)
    {
        var deck = await _context.Decks.FindAsync(id);
        if (deck == null)
        {
            return false;
        }
     
        deck.Status = DeckStatus.Accepted;

        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RejectDeck(int id)
    {
        var deck = await _context.Decks.FindAsync(id);
        if (deck == null)
        {
            return false;
        }
     
        deck.Status = DeckStatus.Rejected;

        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<Deck?> GetById(int id)
    {
        return await _context.Decks.Where(deck => deck.Id == id).Include(deck => deck.Flashcards).FirstOrDefaultAsync();
    }

    public async Task<Deck> Create(CreateDeckDto inputDeckDto, string userId)
    {
        var deck = new Deck
        {
            Name = inputDeckDto.Name,
            UserId = userId,
            SessionLimit = inputDeckDto.SessionLimit,
            DeckType = inputDeckDto.DeckType,
        };

        _context.Decks.Add(deck);
        await _context.SaveChangesAsync();
        return deck;
    }
    
    public async Task<bool> Update(int id, CreateDeckDto inputDeckDto)
    {
        var deck = await _context.Decks.FindAsync(id);
        if (deck == null)
        {
            return false;
        }

        deck.Name = inputDeckDto.Name;
        deck.SessionLimit = inputDeckDto.SessionLimit;
        deck.DeckType = inputDeckDto.DeckType;

        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> Delete(int id)
    { 
        if (!DeckExists(id))
        {
            return false;
        }
        var deck = await _context.Decks.FindAsync(id);
        if (deck == null)
        {
            return false;
        }

        _context.Decks.Remove(deck);
        await _context.SaveChangesAsync();
        return true;
    }
    private bool DeckExists(int id)
    {
        return _context.Decks.Any(e => e.Id == id);
    }
}