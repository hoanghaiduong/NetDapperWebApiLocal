
namespace NetDapperWebApi.Entities
{
    public class Image : BaseEntity<int>
    {
        public string EntityType { get; set; } = null!;
        public int EntityId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsThumbnail { get; set; }
        public int SortOrder { get; set; }
    }
}