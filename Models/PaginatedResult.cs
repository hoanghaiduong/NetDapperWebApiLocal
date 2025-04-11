using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDapperWebApi.Models
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(List<T> items, int totalCount, int currentPage, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
        }
   
        public List<T> Items { get; set; } = [];
        public int TotalCount { get; set; }  // Tổng số lượng bản ghi

        public int PageSize { get; set; }    // Số lượng bản ghi mỗi trang
        public int CurrentPage { get; set; } // Trang hiện tại
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize); // Tổng số trang

        // Các thuộc tính cho trang trước và trang sau
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        // Các trang trước và sau
        public int? PreviousPage => HasPreviousPage ? CurrentPage - 1 : (int?)null;
        public int? NextPage => HasNextPage ? CurrentPage + 1 : (int?)null;

    }
}