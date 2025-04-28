using System.Text.Json.Serialization;

namespace NetDapperWebApi_local.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ERoomStatus
    {
        Empty,      // Phòng trống
        Booked,     // Phòng đã đặt
        Rented      // Phòng đang thuê
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ECleanStatus
    {
        Ready,         // Đã dọn sạch
        Maintenance,   // Đang sửa chữa
        Not_Cleaned    // Chưa dọn dẹp
    }
}
