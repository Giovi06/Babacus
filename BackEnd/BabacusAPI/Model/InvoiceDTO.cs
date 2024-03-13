public class InvoiceDTO
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? SupplierId { get; set; }
    public required DateTime CreatedDate { get; set; }
    public int? DaysTillDueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }

}