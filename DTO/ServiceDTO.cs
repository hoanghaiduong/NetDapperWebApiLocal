using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.DTO
{
    public class ServiceDTO
    {
   
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

    }
}