using IWantAPP.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IWantAPP.Endpoints.Products;

public class ProductGetShowcase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(
        ApplicationDbContext context,
        int offset = 0,
        int limit = 30,
        string orderBy = "name"
    )
    {
        if (limit > 50)
        {
            return Results.Problem(title: "Limit with max 50", statusCode: 400);
        }

        orderBy = string.IsNullOrEmpty(orderBy) ? "name" : orderBy;

        var queryFilter = context.Products.AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category != null && p.Category.Active);
        if (orderBy == "name")
        {
            queryFilter = queryFilter.OrderBy(p => p.Name);
        }
        else
        {
            queryFilter = queryFilter.OrderBy(p => p.Price);
        }
        queryFilter = queryFilter.Skip(offset).Take(limit);

        var products = queryFilter.ToList();

        var response = products.Select(
            p => new ProductResponse
            {
                Name = p.Name,
                CategoryName = p.Category.Name,
                Description = p.Description,
                HasStock = p.HasStock,
                Price = p.Price,
                Active = p.Active,
            });

        return Results.Ok(
            new
            {
                success = true,
                message = "Ok",
                data = response,
            });
    }
}
