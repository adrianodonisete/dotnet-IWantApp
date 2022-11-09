using IWantAPP.Domain.Products;
using IWantAPP.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System;

namespace IWantAPP.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/Categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if (category == null)
        {
            return Results.NotFound();
        }

        category.Edit(categoryRequest.Name, categoryRequest.Active, "edit teste");
        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        context.SaveChanges();

        return Results.Ok(category);
    }
}
