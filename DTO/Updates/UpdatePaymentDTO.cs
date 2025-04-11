using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi.DTO.Updates
{
    public class UpdatePaymentDTO :PaymentDTO
    {
        [JsonIgnore]
        public int InvoiceId {get;set;}
    }
}