public class InvoiceDTO
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? SupplierId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? DaysTillDueDate { get; set; }
    public decimal Amount { get; set; }
    public bool Payed { get; set; }

    // Calculated property for DueDate
    public DateTime DueDate => CalculateDueDate();

    // Method to calculate DueDate
    private DateTime CalculateDueDate()
    {
        return CreatedDate.AddDays(DaysTillDueDate ?? 1);
    }
    public InvoiceDTO()
    {
        // Set default value for CreatedDate if not provided
        if (CreatedDate == default)
        {
            CreatedDate = DateTime.Now;
        }
    }
}