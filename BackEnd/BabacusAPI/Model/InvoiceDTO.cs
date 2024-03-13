public class InvoiceDTO
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Number { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }

}