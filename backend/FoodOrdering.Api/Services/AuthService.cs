using FoodOrdering.Api.Data;
using FoodOrdering.Api.DTOs;
using FoodOrdering.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Api.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokenService;

    public AuthService(AppDbContext db, TokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<(bool Success, AuthResponse? Data, string? Error)> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _db.Users.AnyAsync(u => u.Email == email))
            return (false, null, "Email is already registered.");

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "Customer"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return (true, BuildResponse(user), null);
    }

    public async Task<(bool Success, AuthResponse? Data, string? Error)> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return (false, null, "Invalid email or password.");

        return (true, BuildResponse(user), null);
    }

    private AuthResponse BuildResponse(User user) => new()
    {
        Token = _tokenService.CreateToken(user),
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role
    };
}
