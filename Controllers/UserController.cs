using Flashcards.DTOs;
using Flashcards.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    UserManager<IdentityUser> userManager,
    JwtTokenService jwtTokenService)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> PostUser(CreateUser user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await userManager.CreateAsync(
            new IdentityUser() { UserName = user.UserName, Email = user.Email },
            user.Password
        );

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        user.Password = null;
        return Created("", new CreateUserResponse { UserName = user.UserName, Email = user.Email });
    }

    [HttpPost("BearerToken")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(LoginUser request)
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

        var token = jwtTokenService.CreateToken(user);

        return Ok(token);
    }
}
