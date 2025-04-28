using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi_local.Entities
{
    public class RoomType : BaseEntity<int>
    {

        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int NumberOfBeds { get; set; }
        public int SingleBed { get; set; }
        public int DoubleBed { get; set; }
        public int Capacity { get; set; }
        public int Sizes { get; set; }
        public string? Thumbnail { get; set; }
        [JsonIgnore]
        public string Images { get; set; }
        [NotMapped]
        [JsonPropertyName("Images")]
        public List<string>? ImageList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Images) || !Images.StartsWith("["))
                    return []; // Nếu null hoặc không phải JSON, trả về list rỗng
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Images);
            }
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Room>? Rooms { get; set; } = null;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Amenities> Amenities { get; set; } = [];

    }
}