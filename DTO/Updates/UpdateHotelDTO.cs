

namespace NetDapperWebApi.DTO.Updates
{
    public class UpdateHotelDTO : HotelDTO
    {

        public int Id { get; set; }
        public new string? Name { get; set; }
        public new int? Quantity { get; set; }

        public new string? Description { get; set; }
        public new string? Address { get; set; }
        public new string? Location { get; set; }
        public new string? Phone { get; set; }

        public new string? Email { get; set; }

        public new IFormFile? Thumbnail { get; set; }
        public new List<IFormFile?>? Images { get; set; }
        // ✅ Danh sách ảnh cũ cần giữ lại (chỉ chứa URL)
        public List<string>? KeptImages { get; set; }
        public new int? Stars { get; set; }
        public new string? CheckinTime { get; set; }
        public new string? CheckoutTime { get; set; }
    }
}