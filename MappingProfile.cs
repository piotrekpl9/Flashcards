using AutoMapper;
using Flashcards.DTOs;
using Flashcards.Models;

namespace Flashcards;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Flashcard, FlashcardDto>();
        CreateMap<Flashcard, CreateFlashcardDto>();

        CreateMap<Deck, DeckDto>().ForMember(dto => dto.UserEmail, opt => opt.MapFrom(src => src.User.Email));
        CreateMap<Deck, CreateDeckDto>();

    }
}