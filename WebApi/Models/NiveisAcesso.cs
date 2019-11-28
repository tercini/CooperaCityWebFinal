using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class NiveisAcesso : IdentityRole
    {
        public string Descricao { get; set; }
    }
}
