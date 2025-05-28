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
    
        public async Task<IEnumerable<Flashcard>> GetAll()
        {
            return await _context.Flashcards
                .Include(e => e.User)
                .ToListAsync();
        }
        
        public async Task<Flashcard?> GetById(int id)
        {
            return await _context.Flashcards.Where(f => f.Id == id).FirstOrDefaultAsync();
        }


        public async Task<bool> Update(int id, CreateFlashcardDto inputFlashcardDto, string userId)
        {
            if (!DeckExists(inputFlashcardDto.DeckId) || !FlashcardExists(id))
            {
                return false;                
            }
            var flashcard = new Flashcard()
            {
                Id = id,
                UserId = userId,
                Front = inputFlashcardDto.Front,
                Back = inputFlashcardDto.Back,
                DeckId = inputFlashcardDto.DeckId,
            };

            _context.Entry(flashcard).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Flashcard?> Create(CreateFlashcardDto inputFlashcardDto, string userId)
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
                DeckId = inputFlashcardDto.DeckId,
            };
            if (!DeckExists(inputFlashcardDto.DeckId))
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
        
        private bool DeckExists(int id)
        {
            return _context.Decks.Any(e => e.Id == id);
        }

        private bool FlashcardExists(int id)
        {
            return _context.Flashcards.Any(e => e.Id == id);
        }
    
}