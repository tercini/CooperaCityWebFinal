using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.Helpers
{
    public class IsAutenticatedAtributte : ActionFilterAttribute
    {
        public  override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            context.Result = new RedirectToActionResult("Login", "Usuarios", null);

            if (context.HttpContext.User.IsInRole("Usuario"))
                context.Result = new RedirectToActionResult("Login", "Usuarios", null);
        }    
    }
}
