using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.Common.Enums;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.DTO
{
    public class InvoiceDTO
    {
        public int BookingId { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal ExcessAmount { get; set; }
        [DefaultValue(value: EInvoiceStatus.Pending)]
        public EInvoiceStatus? Status { get; set; } = EInvoiceStatus.Pending;
    }
}