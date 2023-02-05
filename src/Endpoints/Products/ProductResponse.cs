namespace IWantAPP.Endpoints.Products;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public bool HasStock { get; set; }
    public bool Active { get; set; }
    public decimal Price { get; set; }
}
