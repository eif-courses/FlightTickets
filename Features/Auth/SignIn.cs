using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using FlightTickets.Data;
using Microsoft.AspNetCore.Identity;

namespace FlightTickets.Features.Auth;

public class SingInRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignIn: Endpoint<SingInRequest>
{
    private readonly SignInManager<MyUserIdentity> _signInManager;
    private readonly UserManager<MyUserIdentity> _userManager;


    public SignIn(SignInManager<MyUserIdentity> signInManager, UserManager<MyUserIdentity> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("/sign-in");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SingInRequest req, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);
        if (user == null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        var signInResult = await _signInManager.PasswordSignInAsync(req.Email, 
            req.Password, true, false);

        if (!signInResult.Succeeded)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("bestFriend", user.MyBestFriend)
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        await CookieAuth.SignInAsync(u =>
        {
            u.Roles.AddRange(roles);
            u.Claims.AddRange(claims);
        });

        
        await SendAsync("Sekmingai prisijungete",200,ct);
        
    }
}