using DenuncieSpam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.ViewModels
{
    public class ReclamacaoViewModel
    {
        [Key]
        public int IdReclamacao { get; set; }

        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Use menos caracteres")]
        public string Empresa { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "Use menos caracteres")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "Use menos caracteres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(500, ErrorMessage = "Use menos caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Imagem { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Longitude { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Endereco { get; set; }

        //public string Foto { get; set; }

        public string IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}
