using System.Security.Claims;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Services;

public class FlashcardService
{
    private readonly ApplicationDbContext _context;

    public FlashcardService(ApplicationDbContext context)
    {
        _context = context;
    }
    
        public async Task<IEnumerable<Flashcard>> GetAllByUserId(string? userId)
        {
            return await _context.Flashcards
                .Include(e => e.User)
                .Include(flashcard => flashcard.Deck)
                .Where(flashcard => flashcard.Deck.UserId == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Flashcard>> GetAllByDeckId(int deckId)
        {
            return await _context.Flashcards
                .Include(e => e.User)
                .Include(flashcard => flashcard.Deck)
                .Where(flashcard => flashcard.Deck.Id == deckId)
                .ToListAsync();
        }
        public async Task<Flashcard?> GetById(int id)
        {
            return await _context.Flashcards.Where(f => f.Id == id).FirstOrDefaultAsync();
        }


        public async Task<Flashcard?> Update(int id, CreateFlashcardDto inputFlashcardDto, string userId)
        {
            var flashcardDb = await _context.Flashcards
                .Include(f => f.Deck)
                .FirstOrDefaultAsync(ds => ds.Id == id && ds.Deck.UserId == userId);

            if (flashcardDb == null)
                return null;

            flashcardDb.Front = inputFlashcardDto.Front;
            flashcardDb.Back = inputFlashcardDto.Back;

            await _context.SaveChangesAsync();
            return flashcardDb;
        }

        public async Task<Flashcard?> Create(CreateFlashcardDto inputFlashcardDto, string userId, int deckId)
        {
            if (!DeckExists(inputFlashcardDto.DeckId))
            {
                return null;                
            }
            var flashcard = new Flashcard()
            {
                UserId = userId,
                Front = inputFlashcardDto.Front,
                Back = inputFlashcardDto.Back,
                DeckId = deckId,
            };
            if (!DeckExists(deckId))
            {
                return null;
            }
            _context.Flashcards.Add(flashcard);
            await _context.SaveChangesAsync();
            
            
            return await _context.Flashcards.Where(f => f.Id == flashcard.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);
            if (flashcard == null)
            {
                return false;
            }

            _context.Flashcards.Remove(flashcard);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public FlashcardDto MapToFlashcardDto(Flashcard flashcard)
        {
            return new FlashcardDto
            {
                Id = flashcard.Id,
                Front = flashcard.Front,
                Back = flashcard.Back
            };
        }
        
        private bool DeckExists(int id)
        {
            return _context.Decks.Any(e => e.Id == id);
        }
    
}