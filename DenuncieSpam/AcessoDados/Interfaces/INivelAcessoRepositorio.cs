using DenuncieSpam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.AcessoDados.Interfaces
{
    public interface INivelAcessoRepositorio : IRepositorioGenerico<NiveisAcesso>
    {
        Task<bool> NivelAcessoExiste(string nivelAcesso);
    }
}
