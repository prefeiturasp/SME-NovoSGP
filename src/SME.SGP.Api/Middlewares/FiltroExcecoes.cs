using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;

namespace SME.SGP.Api.Middlewares
{
    public class FiltroExcecoes : ExceptionFilterAttribute
    {
        public readonly string sentryDSN;

        public FiltroExcecoes(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new System.ArgumentNullException(nameof(configuration));
            }
            sentryDSN = configuration.GetValue<string>("Sentry:DSN");
        }

        public override void OnException(ExceptionContext context)
        {
            using (SentrySdk.Init(sentryDSN))
            {
                SentrySdk.CaptureException(context.Exception);
            }

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