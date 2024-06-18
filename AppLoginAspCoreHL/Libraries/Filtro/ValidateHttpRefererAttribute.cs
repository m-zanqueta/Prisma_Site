using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginAspCoreHL.Libraries.Filtro
{
    public class ValidateHttpRefererAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Executado antes passar pelo controlador
            // Esta validação verifica se a requisição esta vindo do nosso host 
            string referer = context.HttpContext.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                context.Result = new ContentResult() { Content = "Acesso negado!" };
            }
            else
            {
                Uri uri = new Uri(referer);

                string hostReferer = uri.Host;
                string hostServidor = context.HttpContext.Request.Host.Host;

                if (hostReferer != hostServidor)
                {
                    context.Result = new ContentResult() { Content = "Acesso negado!" };
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Executado após passar pelo controlador
        }
    }
}
