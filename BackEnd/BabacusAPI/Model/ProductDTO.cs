public class ProductDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int SupplierId { get; set; }
    public int Quantity { get; set; }
    public int Stock
    {
        get
        {
            return this.Quantity;
        }
        set
        {
            this.Quantity = value;
        }
    }
}