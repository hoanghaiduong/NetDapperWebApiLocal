using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi.Entities
{
  public class Invoice : BaseEntity<int>
  {

    [Required]
    public int BookingId { get; set; }

    public string InvoiceCode { get; set; }
    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public decimal ExcessAmount { get; set; }
    public string? Status { get; set; }

    // Navigation Properties
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual Booking Booking { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual List<Payment> Payments { get; set; } = [];
  
  }
}