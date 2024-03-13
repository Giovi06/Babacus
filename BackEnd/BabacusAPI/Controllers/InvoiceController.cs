using BabacusAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<InvoiceDTO>>> GetAllInvoices()
        {
            var invoices = await this._context.Invoices.Select(x => InvoiceToInvoiceDTO(x)).ToListAsync();
            return Ok(invoices);
        }

        [HttpGet]
        [Route("getsingleinvoice")]
        public async Task<IActionResult> GetSingleInvoice(int id)
        {
            if (this._context.Invoices == null)
            {
                return this.Problem("Entity set 'Invoices' is null.");
            }
            var invoice = await this._context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }
            return Ok(InvoiceToInvoiceDTO(invoice));
        }

        [HttpPost]
        [Route("createinvoice")]
        public async Task<IActionResult> CreateInvoice(InvoiceDTO invoiceDTO)
        {
            if (invoiceDTO == null)
            {
                return BadRequest("InvoiceDTO is null.");
            }

            if (!(invoiceDTO.DaysTillDueDate > 1) /*|| !(invoiceDTO.DueDate > invoiceDTO.CreatedDate) */ || !(invoiceDTO.Amount > 0)) // Days till due date should be checked of overdue by negative value
            {
                return BadRequest("Invalid invoice data.");
            }
            var invoice = InvoiceDTOToInvoice(invoiceDTO);
            this._context.Invoices.Add(invoice);
            await this._context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSingleInvoice), new { id = invoice.Id }, InvoiceToInvoiceDTO(invoice));
        }
        [HttpPut]
        [Route("updateinvoice")]
        public async Task<IActionResult> UpdateInvoice(int id, InvoiceDTO invoiceDTO)
        {
            if (invoiceDTO == null)
            {
                return BadRequest("InvoiceDTO is null.");
            }
            if (!(invoiceDTO.DueDate > invoiceDTO.CreatedDate) || !(invoiceDTO.Amount > 0))
            {
                return BadRequest("Invalid invoice data.");
            }
            var invoice = await this._context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }
            invoice = ExistingInvoiceDTOToInvoice(invoice, invoiceDTO);
            await this._context.SaveChangesAsync();
            return Ok(InvoiceToInvoiceDTO(invoice));
        }
        [HttpDelete]
        [Route("deleteinvoice")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await this._context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }
            this._context.Invoices.Remove(invoice);
            await this._context.SaveChangesAsync();
            return NoContent();
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
                Amount = invoice.Amount,
                Payed = invoice.Payed
            };
        }
        private static Invoice ExistingInvoiceDTOToInvoice(Invoice invoice, InvoiceDTO invoiceDTO)
        {
            if (invoice == null || invoiceDTO == null)
            {
                throw new ArgumentNullException("Invoice or InvoiceDTO is null.");
            }
            invoice.CustomerId = invoiceDTO.CustomerId;
            invoice.SupplierId = invoiceDTO.SupplierId;
            invoice.CreatedDate = invoiceDTO.CreatedDate;
            invoice.DueDate = invoiceDTO.DueDate;
            invoice.Amount = invoiceDTO.Amount;
            invoice.Payed = invoiceDTO.Payed;
            return invoice;

        }
        private static Invoice InvoiceDTOToInvoice(InvoiceDTO invoiceDTO)
        {
            return new Invoice
            {
                CustomerId = invoiceDTO.CustomerId,
                SupplierId = invoiceDTO.SupplierId,
                CreatedDate = invoiceDTO.CreatedDate,
                DueDate = invoiceDTO.DueDate,
                Amount = invoiceDTO.Amount,
                Payed = invoiceDTO.Payed
            };
        }
    }
}