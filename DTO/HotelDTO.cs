using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.DTO
{
    public class HotelDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
       
        public int? Quantity { get; set; }

        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public string? Phone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        public IFormFile? Thumbnail { get; set; }
        [Required]
        public List<IFormFile>? Images { get; set; }
        [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5.")]
        public int? Stars { get; set; }
        public int? Floor { get; set; }
        public string CheckinTime { get; set; }
        public string CheckoutTime { get; set; }
    }

}