﻿using IWantAPP.Infra.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IWantAPP.Endpoints.Security;

public class TokenPost
{
    public static string Template => "/access-token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(
        LoginRequest loginRequest,
        IConfiguration configuration,
        UserManager<IdentityUser> userManager,
        ILogger<TokenPost> log,
        IWebHostEnvironment environment
    )
    {
        try
        {
            log.LogInformation("Getting token");
            log.LogWarning("warning");

            var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
            if (user == null || !userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
            {
                log.LogError("Login invalid");
                throw new Exception("Login invalid");
            }

            var claims = userManager.GetClaimsAsync(user).Result;
            var subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, loginRequest.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                });
            subject.AddClaims(claims);

            var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = configuration["JwtBearerTokenSettings:Audience"],
                Issuer = configuration["JwtBearerTokenSettings:Issuer"],
                Expires = environment.IsDevelopment() || environment.IsStaging()
                    ? DateTime.UtcNow.AddYears(1)
                    : DateTime.UtcNow.AddHours(1),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var finalToken = tokenHandler.WriteToken(token);

            return Results.Ok(new
            {
                success = true,
                message = "Ok",
                token = finalToken,
                expire = 3600
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new
            {
                success = false,
                message = ex.Message,
                extra = "falhou 2"
            });
        }
    }
}
