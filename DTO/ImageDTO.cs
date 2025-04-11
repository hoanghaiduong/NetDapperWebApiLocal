using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.DTO
{
    public class ImageDTO
    {
        [AllowedValues([$"{nameof(Hotel)}s", $"{nameof(Room)}s", $"{nameof(RoomType)}s"])]
        [Required]
        public string EntityType { get; set; }
        [Required]
        public int EntityId { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public bool IsThumbnail { get; set; } = false;
        public int SortOrder { get; set; }
        
    }
}