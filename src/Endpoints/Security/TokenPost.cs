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

    public static IResult Action(LoginRequest loginRequest, UserManager<IdentityUser> userManager)
    {
        try
        {
            var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
            if (user == null || !userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
            {
                throw new Exception("Login invalid");
            }

            var key = Encoding.ASCII.GetBytes("A@fderwfQQ");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, loginRequest.Email),
                }),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = "IWantApp",
                Issuer = "Issuer"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Results.Ok(new
            {
                token = tokenHandler.WriteToken(token)
            }); ;

        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }
}
