using System.Text.Json.Serialization;

namespace NetDapperWebApi.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EPaymentMethod
    {
        Credit_Card = 1,
        Bank_Transfer = 2,
        PayPal = 3,
        Cash = 4
    }
}