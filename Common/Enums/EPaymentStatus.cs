using System.Text.Json.Serialization;

namespace NetDapperWebApi.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EPaymentStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,//Thanh toán thất bại.
        Refunded = 3//Giao dịch đã được hoàn tiền.
    }
}