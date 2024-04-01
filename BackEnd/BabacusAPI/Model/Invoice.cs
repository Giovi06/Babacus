public class Invoice
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? SupplierId { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required DateTime DueDate { get; set; }
    public required decimal Amount { get; set; }
    public required bool Payed { get; set; }

}