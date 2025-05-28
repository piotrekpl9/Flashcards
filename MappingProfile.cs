using AutoMapper;
using Flashcards.DTOs;
using Flashcards.Models;

namespace Flashcards;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Flashcard, FlashcardDto>();
        CreateMap<PublicFlashcard, PublicFlashcardDto>();

        CreateMap<Deck, DeckDto>();
        CreateMap<PublicDeck, PublicDeckDto>();

    }
}