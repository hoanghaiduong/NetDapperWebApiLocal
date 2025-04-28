using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.DTO
{
    public class ReviewDTO : BaseEntity<int>
    {
        [Required]
        public int Rating { get; set; }

        [Required, MaxLength(500)]
        public string Comment { get; set; }
    }
}