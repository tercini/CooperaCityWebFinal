﻿
using WebApi.AcessoDados.Interfaces;
using WebApi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.AcessoDados.Repositorios
{
    public class UsuarioRepositorio : RepositorioGenerico<Usuario>, IUsuarioRepositorio
    {
        private readonly SignInManager<Usuario> _gerenciadorLogin;
        private readonly UserManager<Usuario> _gerenciadorUsuario;

        public UsuarioRepositorio(Contexto contexto, SignInManager<Usuario> gerenciadorLogin, UserManager<Usuario> gerenciadorUsuario) : base(contexto)
        {
            _gerenciadorLogin = gerenciadorLogin;
            _gerenciadorUsuario = gerenciadorUsuario;
        }

        public async Task<Usuario> PegarUsuarioLogado(ClaimsPrincipal usuario)
        {
            return await _gerenciadorUsuario.GetUserAsync(usuario);
        }

        public async Task<IdentityResult> SalvarUsuario(Usuario usuario, string senha)
        {
            return await _gerenciadorUsuario.CreateAsync(usuario, senha);
        }

        public async Task AtribuirNivelAcesso(Usuario usuario, string nivelAcesso)
        {
            await _gerenciadorUsuario.AddToRoleAsync(usuario, nivelAcesso);
        }

        public async Task EfetuarLogin(Usuario usuario, bool lembrar)
        {
            await _gerenciadorLogin.SignInAsync(usuario, lembrar);
        }

        public async Task EfetuarLogOut()
        {
            await _gerenciadorLogin.SignOutAsync();
        }

        public async Task<Usuario> PegarUsuarioPeloEmail(string email)
        {
            return await _gerenciadorUsuario.FindByEmailAsync(email);
        }

        public async Task AtualizarUsuario(Usuario usuario)
        {
            await _gerenciadorUsuario.UpdateAsync(usuario);
        }
    }
}