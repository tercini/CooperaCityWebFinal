using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DenuncieSpam.Controllers
{
    public class ErrosController : Controller
    {
        [HttpGet("Erros/{codigoErro}")]
        public IActionResult Index(int codigoErro)
        {
            return View(codigoErro);
        }
    }
}