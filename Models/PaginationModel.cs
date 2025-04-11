
using System.ComponentModel;

namespace NetDapperWebApi.Models
{
    public class PaginationModel
    {
        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1; // Trang hiện tại, mặc định là 1
        [DefaultValue(50)]
        public int PageSize { get; set; } = 50;  // Số lượng bản ghi mỗi trang, mặc định là 10
        [DefaultValue(0)]
        public int? Depth { get; set; } = 0;
        public string? Search { get; set; } = string.Empty;  // Từ khóa tìm kiếm tùy chọn
    }
}