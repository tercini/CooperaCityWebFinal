using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Reclamacao
    {
        public int IdReclamacao { get; set; }
        
        public DateTime Data { get; set; }

        public string Empresa { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string Descricao { get; set; }

        //public string Foto { get; set; }

        public string IdUsuario { get; set; }

        public Usuario Usuario { get; set; }    
        
        

    }
}
