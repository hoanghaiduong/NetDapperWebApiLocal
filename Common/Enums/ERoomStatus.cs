using System.Text.Json.Serialization;

namespace NetDapperWebApi.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ERoomStatus
    {
        Not_Available = 0,
        Ready = 1,
        Booked = 2, 
        Assign_Clean = 3,
        Maintenance = 4

    }
}