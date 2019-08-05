using Microsoft.AspNetCore.Mvc.Filters;
using SME.SGP.Dominio;

namespace SME.SGP.Api.Middlewares
{
    public class FiltroExcecoes : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //TODO ADICIONAR LOG DO SENTRY AQUI
            if (context.Exception is NegocioException)
            {
                context.Result = new ResultadoBaseResult(context.Exception.Message);
            }
            else
            {
                context.Result = new ResultadoBaseResult("Ocorreu um erro interno. Favor contatar o suporte.");
            }
            base.OnException(context);
        }
    }
}
