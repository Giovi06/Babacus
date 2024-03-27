public class ProductDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int SupplierId { get; set; }
    public int Quantity
    {
        get { return Quantity; }
        set { this.Stock += value; }
    }
    public int? Stock { get; set; }
}