using AppLoginAspCoreHL.Libraries.Login;
using AppLoginAspCoreHL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppLoginAspCoreHL.Libraries.Filtro
{
    
    public class ClienteAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginCliente _loginCliente;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginCliente = (LoginCliente)context.HttpContext.RequestServices.GetService(typeof(LoginCliente));
            Cliente cliente = _loginCliente.GetCliente(); 
            if(cliente == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
        }
    }
}