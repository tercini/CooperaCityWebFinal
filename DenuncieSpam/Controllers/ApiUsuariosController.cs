using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DenuncieSpam.AcessoDados.Interfaces;
using DenuncieSpam.Models;
using DenuncieSpam.ViewModels;

namespace WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsuariosController : ControllerBase
    {

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<Usuario> _signInManager;

        public ApiUsuariosController(IUsuarioRepositorio usuarioRepositorio, IConfiguration configuration, SignInManager<Usuario> signInManager)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        private async Task<UserToken> BuildToken(LoginViewModel login)
        {
            var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, login.Email),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Role, "Administrador"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(1);            


            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,               
               signingCredentials: creds);
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration                
            };
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginViewModel login)
        {
            //var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha,
            //     isPersistent: true, lockoutOnFailure: false);

            try
            {
                var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
                //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

                var result = await _signInManager.PasswordSignInAsync(usuario.UserName, login.Senha,
                 isPersistent: false, lockoutOnFailure: false);

                //var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
                //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();



                if (result.Succeeded)
                {
                    return await BuildToken(login);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "login inválido.");
                    return BadRequest(ModelState);
                }

            }
            catch(Exception ex)
            {

            }


            return BadRequest(ModelState);


        }

        [HttpPost("LoginInterno")]
        public async Task<int> LoginInterno([FromBody] LoginViewModel login)
        {
            //var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha,
            //     isPersistent: true, lockoutOnFailure: false);

            try
            {
                var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
                //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

                var result = await _signInManager.PasswordSignInAsync(usuario.UserName, login.Senha,
                 isPersistent: false, lockoutOnFailure: false);

                //var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
                //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();



                if (result.Succeeded)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {

            }


            return 0;


        }

        [HttpPost("BuscarIdUsuario")]
        public async Task<string> BuscarIdUsuario([FromBody] LoginViewModel login)
        {
            //var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha,
            //     isPersistent: true, lockoutOnFailure: false);

            try
            {
                var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
                //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

                var result = await _signInManager.PasswordSignInAsync(usuario.UserName, login.Senha,
                 isPersistent: false, lockoutOnFailure: false);

                //var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
                //PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();



                if (result.Succeeded)
                {
                    return usuario.Id;
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception ex)
            {

            }


            return "0";


        }


        [AllowAnonymous]
        [HttpPost("Create")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<UserToken>> Create([FromBody] RegistroViewModel registro)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    UserName = registro.NomeUsuario,
                    Email = registro.Email,
                    CPF = registro.CPF,
                    Telefone = registro.Telefone,
                    Nome = registro.Nome,
                    PasswordHash = registro.Senha
                };

                //_logger.LogInformation("Tentando criar um usuário");
                var resultado = await _usuarioRepositorio.SalvarUsuario(usuario, registro.Senha);


                if (resultado.Succeeded)
                {
                   
                    var nivelAcesso = "Usuario";

                    await _usuarioRepositorio.AtribuirNivelAcesso(usuario, nivelAcesso);
                    
                    await _usuarioRepositorio.EfetuarLogin(usuario, false);

                    return new ObjectResult("cadastrado");

                }
                else
                {
                    return new ObjectResult("falou");
                }

                /*
                var usuario = await _usuarioRepositorio.PegarUsuarioPeloEmail(login.Email);
                PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

                if (usuario != null)
                {
                    if (passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, login.Senha) != PasswordVerificationResult.Failed)
                    {
                        await _usuarioRepositorio.EfetuarLogin(usuario, false);

                        return await BuildToken(login);
                    }
                    return new ObjectResult("falhou");
                }
                */
                
            }
            return new ObjectResult("falhou");
        }

        [Authorize (Roles="Administrador, Usuario")]
        // GET api/values        
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }


        
        // GET api/values/5
        [HttpGet("{id}")]        
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /*
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        */

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
