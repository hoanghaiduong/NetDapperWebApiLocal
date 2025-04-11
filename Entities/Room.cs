using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi.Common.Enums;
using NetDapperWebApi.DTO;

namespace NetDapperWebApi.Entities
{
    public class Room : BaseEntity<int>
    {
        [JsonIgnore]
        public int RoomTypeId { get; set; }
        public string RoomNumber { get; set; }

        public string? Thumbnail { get; set; }
        public int? Floor { get; set; }
        public ERoomStatus? Status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual RoomType? RoomType { get; set; } = null;


        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual ICollection<Booking> Bookings { get; set; } = [];



        // ✅ Cột JSON lưu trữ dưới dạng string trong DB
        [JsonIgnore]
        public string? Images { get; set; }

        // ✅ Chuyển đổi JSON string thành List<string>
        [NotMapped]
        [JsonPropertyName("images")]
        public List<string>? ImageList
        {

            get
            {
                if (string.IsNullOrWhiteSpace(Images) || !Images.StartsWith("["))
                    return []; // Nếu null hoặc không phải JSON, trả về list rỗng
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Images);
            }

        }
    }



}