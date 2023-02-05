using IWantAPP.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IWantAPP.Endpoints.Products;

public class ProductGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(ApplicationDbContext context)
    {
        var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var response = products.Select(p => new ProductResponse
        {
            Name = p.Name,
            CategoryName = p.Category.Name,
            Description = p.Description,
            HasStock = p.HasStock,
            Active = p.Active,
            Price = p.Price,
        });

        return Results.Ok(new
        {
            success = true,
            message = "Ok",
            data = response,
        });
    }
}