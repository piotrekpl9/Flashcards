using System.Security.Claims;
using AutoMapper;
using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Controllers;

public class PublicDecksController: ControllerBase
{
      private readonly ApplicationDbContext _context;
      private readonly PublicDeckService _publicDeckService;
      private readonly IMapper _mapper;

        public PublicDecksController(ApplicationDbContext context, PublicDeckService publicDeckService, IMapper mapper)
        {
            _context = context;
            _publicDeckService = publicDeckService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicDeckDto>>> GetDeck()
        {
            var publicDecks = (await _publicDeckService.GetAll()).Select(deck => _mapper.Map<PublicDeckDto>(deck));
            return publicDecks.ToList();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicDeckDto>> GetDeck(int id)
        {
            var deck = await _publicDeckService.GetById(id);
            return Ok(_mapper.Map<PublicDeckDto>(deck));
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeck(int id, CreatePublicDeckDto inputDeckDto)
        {
            
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PublicDeckDto>> PostDeck(CreatePublicDeckDto inputDeckDto)
        {
            var publicDeck = await _publicDeckService.Create(inputDeckDto, GetUserId());
            return CreatedAtAction("GetPublicDeck", new { id = publicDeck.Id }, _mapper.Map<PublicDeckDto>(publicDeck));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeck(int id)
        {
            var result = await _publicDeckService.Delete(id);
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