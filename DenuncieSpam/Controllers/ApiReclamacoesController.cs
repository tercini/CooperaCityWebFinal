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
using DenuncieSpam.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using WebApi.Controllers;
using System.Net.Http;
using Newtonsoft.Json;
using DenuncieSpam.MapeamentoJson;
using System.IO;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Hosting;

namespace DenuncieSpam.Controllers
{

    //[IsAutenticatedAtributte]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiReclamacoesController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IReclamacaoRepositorio _reclamacaoRepositorio;
        private readonly ILogger<ApiReclamacoesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ApiUsuariosController _apiUsuarios;
        private readonly IHostingEnvironment _appEnvironment;




        public ApiReclamacoesController(IUsuarioRepositorio usuarioRepositorio, IReclamacaoRepositorio reclamacaoRepositorio, ILogger<ApiReclamacoesController> logger, IConfiguration configuration, SignInManager<Usuario> signInManager, ApiUsuariosController apiUsuarios, IHostingEnvironment appEnvironment)
        {
            _configuration = configuration;
            _usuarioRepositorio = usuarioRepositorio;
            _reclamacaoRepositorio = reclamacaoRepositorio;
            _logger = logger;
            _signInManager = signInManager;
            _apiUsuarios = apiUsuarios;
            _appEnvironment = appEnvironment;

            /*
            var usuario = _usuarioRepositorio.PegarUsuarioLogado(User);
            if (usuario == null)
            {
                RedirectToAction("Login", "Usuarios");
            }
            */

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

            //_logger.LogInformation("Listando todos os registros");
            return View( await _reclamacaoRepositorio.PegarTodos());
        }


       
        // GET: Reclamacoes/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Nova Reclamacao");
            
            return  View();
        }


        // GET: Reclamacoes        
        public async Task<List<Reclamacao>> ConsultarTodos()
        {
            //_logger.LogInformation("Listando todos os registros");
            return (List<Reclamacao>) await _reclamacaoRepositorio.PegarTodos();
            //return View(await _reclamacaoRepositorio.PegarTodos());
        }


        [AllowAnonymous]
        [HttpGet("RecuperarPostagens")]
        public async Task<string> RecuperarPostagens([FromHeader] string email, string senha)
        {

            LoginViewModel login = new LoginViewModel();
            login.Email =   email; //reclamacao.Usuario.Email.ToLower();
            login.Senha =   senha; //reclamacao.Usuario.PasswordHash;

            var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);


            //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

            int result = await _apiUsuarios.LoginInterno(login);

            //List<Reclamacao> lst = new List<Reclamacao>();

            //lst = (List<Reclamacao>) await _reclamacaoRepositorio.PegarTodos();

            

            List<Reclamacao> dados = new List<Reclamacao>();
            dados = (List<Reclamacao>) await _reclamacaoRepositorio.PegarTodos();

            Reclamacao rec = null;

            List<Reclamacao> newList = new List<Reclamacao>();

            foreach (var item in dados)
            {
                rec = new Reclamacao();
                rec.Descricao = item.Descricao;
                rec.Data = item.Data;
                rec.Email = item.Email;
                if (item.Endereco != null && item.Endereco != "")
                    rec.Endereco = item.Endereco;
                else
                    rec.Endereco = "";
                rec.IdReclamacao = item.IdReclamacao;
                rec.IdUsuario = item.IdUsuario;
                rec.Imagem = item.Imagem;
                rec.Latitude = item.Latitude;
                rec.Longitude = item.Longitude;
                rec.Telefone = item.Telefone;
                rec.Usuario = null;

                newList.Add(rec);
            }

            //var json = Json(newList);

            string dadosNew = JsonConvert.SerializeObject(newList);

            return dadosNew;

            //return dadosNew;

            //var json = Json(lst);

            //Json

            //return "oi";

            // return View(await  _reclamacaoRepositorio.PegarTodos() );
        }


        [AllowAnonymous]
        [HttpGet("Delete")]
        public async Task<int> Delete([FromHeader] string idReclamacao)
        {
            await _reclamacaoRepositorio.Excluir(Convert.ToInt32(idReclamacao));

            return 1;

            //return dadosNew;

            //var json = Json(lst);

            //Json

            //return "oi";

            // return View(await  _reclamacaoRepositorio.PegarTodos() );
        }

        // POST: Reclamacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public async Task<IActionResult> Create([Bind("IdReclamacao,Data,Empresa,Telefone,Email,Descricao,IdUsuario")] Reclamacao reclamacao)
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> Create([FromBody] Reclamacao reclamacao)        
        {


            //var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(reclamacao.Usuario.Email);
            //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

            //var result = await ApiUsuariosController_signInManager.PasswordSignInAsync(reclamacao.Usuario.UserName, reclamacao.Usuario.PasswordHash,
            // isPersistent: false, lockoutOnFailure: false);

            LoginViewModel login = new LoginViewModel();
            login.Email = reclamacao.Usuario.Email.ToLower();
            login.Senha = reclamacao.Usuario.PasswordHash;

            var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);


            //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

            int result = await _apiUsuarios.LoginInterno(login);

            if (result == 0)
                return View(reclamacao);


            // await _signInManager.PasswordSignInAsync(usuario.UserName, login.Senha,
            //isPersistent: false, lockoutOnFailure: false);

            //await BuildToken(login);


            if (ModelState.IsValid && usuario != null)
            {
                string endereco = "";
                string numero = "";
                string bairro = "";
                string cidade = "";
                string estado = "";
                string cep = "";
                string img = "";
                
                //
                try
                {
                    HttpClient client = new HttpClient();
                    var response = await client.GetStringAsync("https://maps.googleapis.com/maps/api/geocode/json?latlng="+reclamacao.Latitude+","+reclamacao.Longitude+"&key=AIzaSyCZWaCsDUe9eZwKkuz5Zzr4l5_k1iERHwY");

                    //var response = await client.GetStringAsync("https://maps.googleapis.com/maps/api/geocode/json?latlng=-21.2716850,-48.4966450&key=AIzaSyCZWaCsDUe9eZwKkuz5Zzr4l5_k1iERHwY");
                    //var endereco = JsonConvert.DeserializeObject(response);
                    //var json_serializer = new JavaScriptSerializer();
                    //var routes_list = (IDictionary<string, object>)json_serializer.DeserializeObject("{ \"test\":\"some data\" }");
                    //Console.WriteLine(routes_list["test"]);
                    //var item = JsonConvert.DeserializeObject<object>(response);


                    ApiGeocoding end = new ApiGeocoding();

                    ApiGeocoding item = JsonConvert.DeserializeObject<ApiGeocoding>(response);



                    //var teste = JsonConvert.DeserializeObject <String> ( item["results"].ToString());

                    //object teste2 = teste["formatted_address"];

                    //var item2 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(colecao.ToString());


                    //var colecao = item.Where(d => d.Key == "results").Select(d => d.Value);



                    //var colecao2 = colecao.Where(e => e.Key == "address_components").Select(e => e.Value);



                    //string teste = item["formatted_address"];


                    //var item2 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(item.ToString());

                    numero = item.results[0].address_components[0].long_name.ToString(); //numero
                    endereco = item.results[0].address_components[1].long_name.ToString(); //endereco
                    bairro = item.results[0].address_components[2].long_name.ToString(); //bairro
                    cidade = item.results[0].address_components[3].long_name.ToString(); //cidade
                    estado = item.results[0].address_components[4].short_name.ToString(); //estado
                    item.results[0].address_components[5].long_name.ToString(); //pais
                    cep = item.results[0].address_components[6].long_name.ToString(); //cep


                    

                    byte[] bytes = Convert.FromBase64String(reclamacao.Imagem);

                    using (MemoryStream myMemStream = new MemoryStream(bytes))
                    {
                        System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
                        
                        var width = 800;
                        
                        var height = 800;
                        System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(width, height, null, IntPtr.Zero);

                        using (var mst = new MemoryStream())
                        {
                            newImage.Save(mst, System.Drawing.Imaging.ImageFormat.Png);
                            bytes = mst.ToArray();
                        }





                        //bytes = newImage.to

                        using (var myResult = new MemoryStream())
                        {



                            //newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Png);

                            //string arqImg = "../DenuncieSpam/wwwroot/images/incidentes/" + System.Guid.NewGuid() + ".png";
                            string arqImg = _appEnvironment.WebRootPath + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "incidentes" + Path.DirectorySeparatorChar + System.Guid.NewGuid().ToString() + ".png"; //"/images/incidentes/" + System.Guid.NewGuid() + ".png";

                            while (System.IO.File.Exists(arqImg))
                            {
                                //arqImg = "../DenuncieSpam/wwwroot/images/incidentes/" + System.Guid.NewGuid() + ".png";
                                arqImg = _appEnvironment.WebRootPath + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "incidentes" + Path.DirectorySeparatorChar + System.Guid.NewGuid().ToString() + ".png";
                                //arqImg = "/images/incidentes/" + System.Guid.NewGuid() + ".png";
                            }

                            System.IO.File.WriteAllBytes(arqImg, bytes);

                            reclamacao.Imagem = arqImg;

                        }
                    }


                    //System.IO.Directory.GetFiles("../DenuncieSpam/wwwroot/images/incidentes/System.Guid.NewGuid().png");
                    

                }
                catch(Exception ex)
                {

                }
                reclamacao.Endereco = endereco + ", " + numero + ", " + bairro + ", " + cidade + ", " + estado + ", " + cep;
                
                reclamacao.Data = DateTime.Now;
                reclamacao.Telefone = usuario.Telefone;
                reclamacao.Email = usuario.Email;
                reclamacao.IdUsuario = usuario.Id;
                await _reclamacaoRepositorio.Inserir(reclamacao);
                _logger.LogInformation("Nova Reclamacao cadastrada");
                
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
        public async Task<IActionResult> Deletex(int id)
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
