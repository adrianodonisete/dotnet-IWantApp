using IWantAPP.Utils;
using Microsoft.AspNetCore.Identity;

namespace IWantAPP.Endpoints.Employees;

public class EmployeeGetClaim
{
    public static string Template => "/employees/claim";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int offset, int limit, UserManager<IdentityUser> userManager)
    {
        var users = userManager.Users.Skip(offset).Take(limit).ToList();

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
