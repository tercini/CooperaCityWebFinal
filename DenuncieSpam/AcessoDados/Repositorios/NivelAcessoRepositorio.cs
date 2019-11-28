using DenuncieSpam.AcessoDados.Repositorios;
using DenuncieSpam.AcessoDados.Interfaces;
using DenuncieSpam.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.AcessoDados.Repositorios
{
    public class NivelAcessoRepositorio : RepositorioGenerico<NiveisAcesso>, INivelAcessoRepositorio
    {

        private readonly RoleManager<NiveisAcesso> _gerenciadorNiveisAcesso;
        private readonly Contexto _contexto;

        public NivelAcessoRepositorio(Contexto contexto, RoleManager<NiveisAcesso> gerenciadorNiveisAcesso) : base(contexto)
        {
            _contexto = contexto;
            _gerenciadorNiveisAcesso = gerenciadorNiveisAcesso;
        }

        public async Task<bool> NivelAcessoExiste(string nivelAcesso)
        {
            return await _gerenciadorNiveisAcesso.RoleExistsAsync(nivelAcesso);
        }
    }
}
