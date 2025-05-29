using AutoMapper;
using Flashcards.DTOs;
using Flashcards.Models;

namespace Flashcards;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Flashcard, FlashcardDto>();

        CreateMap<Deck, DeckDto>();
        CreateMap<Deck, CreateDeckDto>();

    }
}