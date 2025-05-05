using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDapperWebApi_local.Entities
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ServiceTypeDTO
    {
        public string? Name { get; set; }
    }

}