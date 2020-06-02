using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Excecoes;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SME.SGP.Api.Middlewares
{
    public class FiltroExcecoesAttribute : ExceptionFilterAttribute
    {
        public readonly string sentryDSN;

        public FiltroExcecoesAttribute(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            sentryDSN = configuration.GetValue<string>("Sentry:DSN");
        }

        public override void OnException(ExceptionContext context)
        {
            using (SentrySdk.Init(sentryDSN))
            {
                var internalIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList?.Where(c => c.AddressFamily == AddressFamily.InterNetwork).ToString();
                SentrySdk.AddBreadcrumb($"{Environment.MachineName ?? string.Empty} - {internalIP ?? string.Empty }", "Machine Identification");

                SentrySdk.CaptureException(context.Exception);
            }

            switch (context.Exception)
            {
                case NegocioException negocioException:
                    context.Result = new ResultadoBaseResult(context.Exception.Message, negocioException.StatusCode);
                    break;
                case ValidacaoException validacaoException:
                    context.Result = new ResultadoBaseResult(new RetornoBaseDto(validacaoException.Erros));
                    break;
                default:
                    context.Result = new ResultadoBaseResult("Ocorreu um erro interno. Favor contatar o suporte.", 500);
                    break;
            }

            base.OnException(context);
        }
    }
}