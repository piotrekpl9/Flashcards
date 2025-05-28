namespace Flashcards.DTOs;

public class AuthenticationDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}