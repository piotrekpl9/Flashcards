using System.Security.Claims;
using AutoMapper;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Controllers;

public class FlashcardController: ControllerBase
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
        public async Task<ActionResult<IEnumerable<FlashcardDto>>> GetFlashcard()
        {
            var dtos = (await _flashcardService.GetAll()).Select(flashcard => _mapper.Map<FlashcardDto>(flashcard));
            return dtos.ToList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Deck>> GetFlashcard(int id)
        {
            var flashcard = await _flashcardService.GetById(id);

            if (flashcard == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<FlashcardDto>(flashcard);
         
            return Ok(dto);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlashcard(int id, CreateFlashcardDto inputFlashcardDto)
        {
            var result = await _flashcardService.Update(id, inputFlashcardDto, GetUserId());
            if (!result)
            {
                return BadRequest();    
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<DeckDto>> PostFlashcard(CreateFlashcardDto inputFlashcardDto)
        {
            
            var flashcard = await _flashcardService.Create(inputFlashcardDto, GetUserId());
            if (flashcard == null)
            {
                return BadRequest();
            }
            var dto = _mapper.Map<FlashcardDto>(flashcard);
            return CreatedAtAction("GetFlashcard", new { id = flashcard.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlashcard(int id)
        {
            var result = await _flashcardService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
}