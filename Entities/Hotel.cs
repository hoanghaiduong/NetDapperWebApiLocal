using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NetDapperWebApi.Entities
{
    public class Hotel : BaseEntity<int>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; } = null!;
        public string? Thumbnail { get; set; }
        [JsonIgnore]
        public string? Images { get; set; }
        public int? Floor { get; set; }
        public int? Stars { get; set; }
        public string? CheckinTime { get; set; }
        public string? CheckoutTime { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual IList<User>? Users { get; set; } = null;//employee,guest,admin

        // ✅ Chuyển đổi JSON string thành List<string>
        [NotMapped]
        [JsonPropertyName("images")]
        public List<string> ImageList
        {
            get
            {
                if (string.IsNullOrEmpty(Images) || !Images.StartsWith("["))
                {
                    return [];
                }
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Images);
            }
        }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public List<RoomType> RoomTypes { get; set; } = new List<RoomType>();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<Amenities> Amenities { get; set; } = new List<Amenities>();
    }

}