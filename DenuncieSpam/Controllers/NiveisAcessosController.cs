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
using DenuncieSpam.AcessoDados.Interfaces;
using DenuncieSpam.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DenuncieSpam.Controllers
{    
    //[IsAutenticatedAtributte]
    [Authorize(Roles = "Administrador")]    
    public class NiveisAcessosController : Controller
    {
        private readonly INivelAcessoRepositorio _nivelAcessoRepositorio;
        private readonly ILogger<NiveisAcessosController> _logger;

        public NiveisAcessosController(INivelAcessoRepositorio nivelAcessoRepositorio, ILogger<NiveisAcessosController> logger)
        {
            _nivelAcessoRepositorio = nivelAcessoRepositorio;
            _logger = logger;
        }

        // GET: NiveisAcessos
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Listando todos os registros");
            return View(await _nivelAcessoRepositorio.PegarTodos().ToListAsync());
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Iniciando criação de niveis de acesso");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descricao,Name")] NiveisAcesso niveisAcesso)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Verificando se o nível de acesso existe");
                bool nivelExiste = await _nivelAcessoRepositorio.NivelAcessoExiste(niveisAcesso.Name);

                if (!nivelExiste)
                {
                    niveisAcesso.NormalizedName = niveisAcesso.Name.ToUpper();
                    await _nivelAcessoRepositorio.Inserir(niveisAcesso);
                    _logger.LogInformation("Novo nivel de acesso criado");

                    return RedirectToAction("Index", "NiveisAcessos");
                }
            }

            _logger.LogError("Informações inválidas");
            return View(niveisAcesso);
        }

        // GET: NiveisAcessos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            _logger.LogInformation("Atualizando nível de acesso");
            if (id == null)
            {
                _logger.LogInformation("Nível não encontrado");
                return NotFound();
            }

            var niveisAcesso = await _nivelAcessoRepositorio.PegarPeloId(id);
            if (niveisAcesso == null)
            {
                return NotFound();
            }
            return View(niveisAcesso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Descricao,Id,Name,NormalizedName,ConcurrencyStamp")] NiveisAcesso niveisAcesso)
        {
            if (id != niveisAcesso.Id)
            {
                _logger.LogInformation("Nível não encontrado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                await _nivelAcessoRepositorio.Atualizar(niveisAcesso);
                _logger.LogInformation("Nível atualizado");
                return RedirectToAction("Index", "NiveisAcessos");

            }

            _logger.LogError("Informações inválidas");
            return View(niveisAcesso);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _nivelAcessoRepositorio.Excluir(id);
            _logger.LogInformation("Nível excluído");
            return RedirectToAction(nameof(Index));
        }
    }
}