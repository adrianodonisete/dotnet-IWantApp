using IWantAPP.Infra.Data;
using Microsoft.AspNetCore.Authorization;

namespace IWantAPP.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(int? offset, int? limit, QueryAllUsersWithClaims query)
    {
        var employees = await query.Execute(offset, limit);

        return Results.Ok(new
        {
            success = true,
            message = "Ok",
            record_account = employees.Count(),
            data = employees
        });
    }
}
