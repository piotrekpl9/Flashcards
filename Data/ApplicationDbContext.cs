using Flashcards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<Deck> Decks { get; set; } = default!;
    public DbSet<DeckSession> DeckSessions { get; set; } = default!;
    public DbSet<Flashcard> Flashcards { get; set; } = default!;
    public DbSet<FlashcardsQueue> FlashcardsQueues { get; set; } = default!;
}