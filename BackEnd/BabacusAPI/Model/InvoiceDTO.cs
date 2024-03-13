public class InvoiceDTO
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? SupplierId { get; set; }
    public required DateTime CreatedDate { get; set; }
    public int? DaysTillDueDate { get; set; }
    public DateTime DueDate
    {
        get { return this.CreatedDate.AddDays(this.DaysTillDueDate ?? 30); }
        set { }
    }

    public required decimal Amount { get; set; }
    public required bool Payed { get; set; }

}