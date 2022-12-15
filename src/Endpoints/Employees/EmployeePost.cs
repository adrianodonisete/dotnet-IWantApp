using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantAPP.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequest employeeRequest, HttpContext http, UserManager<IdentityUser> userManager)
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = employeeRequest.Email,
                Email = employeeRequest.Email
            };

            var result = await userManager.CreateAsync(user, employeeRequest.Password);
            if (!result.Succeeded)
            {
                return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
            }

            var userIdAuth = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userClaims = new List<Claim>
            {
                new Claim("EmployeeCode", employeeRequest.EmployeeCode),
                new Claim("Name", employeeRequest.Name),
                new Claim("CreatedBy", userIdAuth),
            };

            var claimResult = await userManager.AddClaimsAsync(user, userClaims);
            if (!claimResult.Succeeded)
            {
                await userManager.DeleteAsync(user);

                return Results.ValidationProblem(claimResult.Errors.ConvertToProblemDetails());
            }

            return Results.Created($"/employees/{user.Id}", user.Id);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
