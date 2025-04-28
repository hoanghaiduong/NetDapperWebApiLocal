using System.Text.Json.Serialization;

namespace NetDapperWebApi_local.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EBookingStatus
    {
        Pending,
        Confirmed,
        Checked_In,
        Checked_Out,
        Cancelled,
        No_Show,
    }
}