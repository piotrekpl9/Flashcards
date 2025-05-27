using System.Security.Claims;
using Flashcards.DTOs;
using Flashcards.Models;
using Flashcards.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    UserManager<User> userManager,
    TokenProvider tokenProvider)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> PostUser(CreateUser user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userExist = await userManager.FindByNameAsync(user.UserName) != null || await userManager.FindByEmailAsync(user.UserName) != null;
        if (userExist)
        {
            return BadRequest();
        }
        
        var result = await userManager.CreateAsync(
            new User { UserName = user.UserName, Email = user.Email },
            user.Password
        );

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Created("", new CreateUserResponse { UserName = user.UserName, Email = user.Email });
    }

    [HttpPost("BearerToken")]
    public async Task<ActionResult<string>> CreateBearerToken(LoginUser request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Bad credentials");
        }

        var user = await userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var token = tokenProvider.Create(user);

        return Ok(token);
    }
    
  
}


