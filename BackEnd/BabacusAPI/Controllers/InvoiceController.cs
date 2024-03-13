using Microsoft.AspNetCore.Mvc;

namespace BabacusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly BabacusDb _context;

        public InvoiceController(BabacusDb context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getallinvoices")]
        public async Task<IActionResult> GetAllInvoices()
        {
            return await this._context.Autos.Select(x => InvoiceToInvoiceDTO(x)).ToListAsync();
        }

        [HttpGet]
        [Route("getsingleinvoice")]
        public async Task<IActionResult> GetSingleInvoice(int id)
        {
            var invoice = await this._context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return InvoiceToInvoiceDTO(invoice);
        }

        [HttpPost]
        [Route("createinvoice")]
        public async Task<IActionResult> CreateInvoice(InvoiceDTO invoiceDTO)
        {
            if (invoiceDTO == null)
            {
                return BadRequest();
            }
            if (!(invoiceDTO.DueDate > invoiceDTO.CreatedDate) || !(invoiceDTO.Amount > 0))
            {
                return BadRequest();
            }
            var invoice = new Invoice
            {
                CustomerId = invoiceDTO.CustomerId,
                SupplierId = invoiceDTO.SupplierId,
                CreatedDate = invoiceDTO.CreatedDate,
                Amount = invoiceDTO.Amount
            };
            this._context.Invoices.Add(invoice);
            await this._context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSingleInvoice), new { id = invoice.Id }, InvoiceToInvoiceDTO(invoice));
        }
        private static InvoiceDTO InvoiceToInvoiceDTO(Invoice invoice)
        {
            return new InvoiceDTO
            {
                Id = invoice.Id,
                CustomerId = invoice.CustomerId,
                SupplierId = invoice.SupplierId,
                CreatedDate = invoice.CreatedDate,
                DueDate = invoice.DueDate,
                Amount = invoice.Amount
            };
        }
        private static Invoice InvoiceDTOToInvoice(InvoiceDTO invoiceDTO)
        {
            return new Invoice
            {
                CustomerId = invoiceDTO.CustomerId,
                SupplierId = invoiceDTO.SupplierId,
                CreatedDate = invoiceDTO.CreatedDate,
                DueDate = invoiceDTO.CreatedDate.AddDays(invoiceDTO.DaysTillDueDate ?? 30),
                Amount = invoiceDTO.Amount

            };
        }
    }
}