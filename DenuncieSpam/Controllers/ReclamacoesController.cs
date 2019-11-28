using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DenuncieSpam.Models;
using DenuncieSpam.AcessoDados.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using DenuncieSpam.Helpers;

namespace DenuncieSpam.Controllers
{
    
    //[IsAutenticatedAtributte]
    [Authorize (Roles ="Administrador, Usuario")]    
    public class ReclamacoesController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IReclamacaoRepositorio _reclamacaoRepositorio;
        private readonly ILogger<ReclamacoesController> _logger;


        public ReclamacoesController(IUsuarioRepositorio usuarioRepositorio, IReclamacaoRepositorio reclamacaoRepositorio, ILogger<ReclamacoesController> logger)
        {
            
            _usuarioRepositorio = usuarioRepositorio;
            _reclamacaoRepositorio = reclamacaoRepositorio;
            _logger = logger;

            var usuario = _usuarioRepositorio.PegarUsuarioLogado(User);
            if (usuario == null)
            {                
                RedirectToAction("Login", "Usuarios");
            }            
        }

        
        
        // GET: Reclamacoes        
        public async Task<IActionResult> Index()
        {
            var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);
            if(usuario != null)
            {
                if(User.IsInRole("Usuario"))
                    return View(await _reclamacaoRepositorio.PegarTodosPorUsuario(usuario.Id));
            }

            var rec = await _reclamacaoRepositorio.PegarTodos();
            //_logger.LogInformation("Listando todos os registros");
            return View( rec );
        }


        // GET: Reclamacoes/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Nova Reclamacao");
            
            return  View();
        }

        // POST: Reclamacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReclamacao,Data,Empresa,Telefone,Email,Descricao,IdUsuario")] Reclamacao reclamacao)
        {
            var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);
            if (ModelState.IsValid && usuario != null)
            {                
                reclamacao.IdUsuario = usuario.Id;
                await _reclamacaoRepositorio.Inserir(reclamacao);
                _logger.LogInformation("Nova Reclamacao cadastrada");
                return RedirectToAction("Index", "Usuarios");
            }

            _logger.LogError("Erro no cadastro de reclamacao");
            return View(reclamacao);
        }

        // GET: Reclamacoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            _logger.LogInformation("Atualizando Reclamacao");            
            
            var reclamacao =  await _reclamacaoRepositorio.PegarPeloId(id);

            if (reclamacao == null)
            {
                return NotFound();
            }
         
            return View(reclamacao);
            
        }

        // POST: Reclamacoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReclamacao,Data,Empresa,Telefone,Email,Descricao,IdUsuario")] Reclamacao reclamacao)
        {
            var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);

            if (id != reclamacao.IdReclamacao && reclamacao.IdUsuario != usuario.Id)
            {
                _logger.LogError("Reclamação não encontrada");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _reclamacaoRepositorio.Atualizar(reclamacao);
                _logger.LogInformation("Endereço atualizado");
                return RedirectToAction("Index", "Usuarios");
            }

            _logger.LogError("Reclamação não encontrada");
            return View(reclamacao);
        }
        

        // POST: Reclamacoes/Delete/5
        [HttpPost]        
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);

            var reclamacao = await _reclamacaoRepositorio.PegarPeloId(id);

            if(usuario.Id != reclamacao.IdUsuario && !User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index");
            }

            await _reclamacaoRepositorio.Excluir(id);
            //return Json("Reclamação excluída");
            _logger.LogInformation("Reclamação excluída");
            return RedirectToAction("Index");
        }

    }
}
