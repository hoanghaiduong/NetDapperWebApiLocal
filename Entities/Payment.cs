using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi.Common.Enums;

namespace NetDapperWebApi.Entities
{
    public class Payment : BaseEntity<int>
    {

        [Required]
        public decimal Amount { get; set; }

        public EPaymentMethod? PaymentMethod { get; set; }
        public EPaymentStatus? Status { get; set; }

        public DateTime? PaymentDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// mapping with id
        /// </summary>
        [JsonIgnore]
        public int InvoiceId { get; set; }

    }
}