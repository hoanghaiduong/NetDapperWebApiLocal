using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.DTO
{
    public class UserRoleDTO
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int RoleId { get; set; }

    }
}