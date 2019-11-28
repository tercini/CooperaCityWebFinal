using WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.AcessoDados.Interfaces
{
    public interface INivelAcessoRepositorio : IRepositorioGenerico<NiveisAcesso>
    {
        Task<bool> NivelAcessoExiste(string nivelAcesso);
    }
}
