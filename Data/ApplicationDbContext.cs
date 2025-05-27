using Flashcards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Data;

public class ApplicationDbContext : IdentityUserContext<User>
{
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Deck> Decks { get; set; } = default!;
    public DbSet<PublicDeck> PublicDecks { get; set; } = default!;
    public DbSet<DeckSession> DeckSessions { get; set; } = default!;
    public DbSet<Flashcard> Flashcards { get; set; } = default!;
    public DbSet<PublicFlashcard> PublicFlashcards { get; set; } = default!;
    public DbSet<FlashcardsQueue> FlashcardsQueues { get; set; } = default!;
}