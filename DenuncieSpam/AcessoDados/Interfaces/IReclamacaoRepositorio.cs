using DenuncieSpam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.AcessoDados.Interfaces
{
    public interface IReclamacaoRepositorio : IRepositorioGenerico<Reclamacao>
    {
        new Task<IEnumerable<Reclamacao>> PegarTodos();
        new Task<IEnumerable<Reclamacao>> PegarTodosPorUsuario(string idUsuario);
        Task<Reclamacao> PegarPeloIdEUsuario(int id, string idUsuario);
    }
}
