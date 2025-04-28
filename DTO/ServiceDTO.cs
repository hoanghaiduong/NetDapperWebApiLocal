using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.DTO
{
    public class ServiceDTO
    {
   
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

    }
}