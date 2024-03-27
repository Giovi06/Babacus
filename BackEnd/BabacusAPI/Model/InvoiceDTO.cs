public class InvoiceDTO
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? SupplierId { get; set; }
    public required DateTime CreatedDate { get; set; }
    public int? DaysTillDueDate { get { return DaysTillDueDate; } set { this.DueDate = this.CreatedDate.AddDays(DaysTillDueDate ?? 1); } }
    public DateTime DueDate { get; set; }
    public required decimal Amount { get; set; }
    public required bool Payed { get; set; }

}