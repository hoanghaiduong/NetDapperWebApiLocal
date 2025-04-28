
using System.Text.Json.Serialization;

namespace NetDapperWebApi_local.Entities
{
    public class Amenities : BaseEntity<int>
    {

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }

    }
}