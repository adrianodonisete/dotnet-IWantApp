using Flunt.Validations;

namespace IWantAPP.Domain.Products;

public class Product : Entity
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public bool HasStock { get; private set; }
    public bool Active { get; private set; } = true;
    public decimal Price { get; private set; }

    public Product() { }

    public Product(string name, Category category, string description, bool hasStock, decimal price, string createdBy)
    {
        Name = name;
        Description = description;
        Category = category;
        HasStock = hasStock;
        Price = price;

        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Product>()
                        .IsNotNullOrEmpty(Name, "Name")
                        .IsGreaterOrEqualsThan(Name, 3, "Name")
                        .IsNotNull(Category, "Category", "Category not found")
                        .IsNotNullOrEmpty(Description, "Description")
                        .IsGreaterOrEqualsThan(Description, 3, "Description")
                        .IsGreaterOrEqualsThan(Price, 0.5, "Price")
                        .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
                        .IsNotNullOrEmpty(EditedBy, "EditedBy");
        AddNotifications(contract);
    }


}
