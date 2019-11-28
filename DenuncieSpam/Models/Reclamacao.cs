using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.Models
{
    public class Reclamacao
    {
        public int IdReclamacao { get; set; }
        
        public DateTime Data { get; set; }
        
        public string Telefone { get; set; }

        public string Email { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Endereco { get; set; }

        public string IdUsuario { get; set; }

        public Usuario Usuario { get; set; }    
        
        

    }
}
