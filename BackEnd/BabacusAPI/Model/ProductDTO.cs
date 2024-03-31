public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int SupplierId { get; set; }
    private int _quantity; // Private backing field for Quantity

    // Quantity property with correct implementation
    public int Quantity
    {
        get { return _quantity; }
        set
        {
            _quantity = value;
            Stock += value; // Update Stock when Quantity is set
        }
    }

    public int Stock { get; set; }
}
