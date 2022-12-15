using Dapper;
using IWantAPP.Endpoints.Employees;
using IWantAPP.Infra.Config;
using Microsoft.Data.SqlClient;

namespace IWantAPP.Infra.Data;

public class QueryAllUsersWithClaims
{
    private readonly SqlConnection db;

    public QueryAllUsersWithClaims()
    {
        db = new Connection().GetConnection();
    }

    public async Task<IEnumerable<EmployeeResponse>> Execute(int? offset, int? limit)
    {
        offset = offset ?? 1;
        limit = limit ?? 10;

        var query =
            @"  SELECT u.Email, n.ClaimValue [Name], co.ClaimValue EmployeeCode
                FROM AspNetUsers u
                    LEFT JOIN AspNetUserClaims n ON n.UserId = u.Id AND n.ClaimType = 'Name'
                    LEFT JOIN AspNetUserClaims co ON co.UserId = u.Id AND co.ClaimType = 'EmployeeCode'
                ORDER BY [Name] ASC
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

        return await db.QueryAsync<EmployeeResponse>(
            query,
            new { offset, limit }
        );
    }
}
