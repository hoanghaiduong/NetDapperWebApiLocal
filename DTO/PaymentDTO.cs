using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.DTO
{
    public class PaymentDTO
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        [JsonIgnore]
        public string? PaymentMethod { get; set; }
        [JsonIgnore]
        public string? Status { get; set; }
        [JsonIgnore]
        public DateTime? PaymentDate { get; set; } = DateTime.Now;
    }
}