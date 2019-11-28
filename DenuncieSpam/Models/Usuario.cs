using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.Models
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }

        public string CPF { get; set; }

        public string Telefone { get; set; }

        public ICollection<Reclamacao> Reclamacoes { get; set; }

        //public ICollection<Aluguel> Alugueis { get; set; }
    }
}
