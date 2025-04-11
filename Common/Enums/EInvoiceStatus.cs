using System.Text.Json.Serialization;

namespace NetDapperWebApi.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EInvoiceStatus
    {
        /// <summary>
        /// Hóa đơn mới được tạo ra và đang chờ thanh toán.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Hóa đơn đã được thanh toán đầy đủ.
        /// </summary>
        Paid = 1,

        /// <summary>
        /// Hóa đơn đã quá hạn thanh toán.
        /// </summary>
        Overdue = 2
    }
}