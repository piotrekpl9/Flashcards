using System.ComponentModel.DataAnnotations;

namespace Flashcards.DTOs;

public class LoginUser
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}