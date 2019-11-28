using DenuncieSpam.AcessoDados.Interfaces;
using DenuncieSpam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DenuncieSpam.AcessoDados.Repositorios
{

    public class ReclamacaoRepositorio : RepositorioGenerico<Reclamacao>, IReclamacaoRepositorio
    {
        private readonly Contexto _contexto;

        public ReclamacaoRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<Reclamacao> PegarPeloIdEUsuario(int id, string idUsuario)
        {
            return (Reclamacao) _contexto.Reclamacoes.Where(x => x.Usuario.Id == idUsuario && x.IdReclamacao == id);
        }

        public new async Task<IEnumerable<Reclamacao>> PegarTodos ()
        {            
            return await _contexto.Reclamacoes.Include(r => r.Usuario).OrderByDescending(x => x.Data) .ToListAsync();
        }

        public new async Task<IEnumerable<Reclamacao>>  PegarTodosPorUsuario(string idUsuario)
        {

            return _contexto.Reclamacoes.Where(x => x.Usuario.Id == idUsuario); 
        }
   
    }
}
