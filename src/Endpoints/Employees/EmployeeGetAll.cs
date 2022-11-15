using IWantAPP.Utils;
using Microsoft.AspNetCore.Identity;

namespace IWantAPP.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(UserManager<IdentityUser> userManager)
    {
        var users = userManager.Users.ToList();

        var employees = new List<EmployeeResponse>();
        foreach (var user in users)
        {
            var claims = userManager.GetClaimsAsync(user).Result;

            var userName = ClaimUtil.GetClaim(claims, "Name");
            var userCode = ClaimUtil.GetClaim(claims, "EmployeeCode");

            employees.Add(new EmployeeResponse(userName, user.Email, userCode));
        }
        return Results.Ok(employees);
    }
}
