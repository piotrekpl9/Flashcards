using System.ComponentModel.DataAnnotations;

namespace Flashcards.DTOs;

public class CreateUserDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}