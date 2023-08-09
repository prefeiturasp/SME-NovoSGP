using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Excecoes;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SME.SGP.Api.Middlewares
{
    public class FiltroExcecoesAttribute : ExceptionFilterAttribute
    {
        private readonly IMediator mediator;
        private readonly ILogger<FiltroExcecoesAttribute> logger;

        public FiltroExcecoesAttribute(IMediator mediator, ILogger<FiltroExcecoesAttribute> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger;
        }

        public override async void OnException(ExceptionContext context)
        {
            var internalIP = "";

            switch (context.Exception)
            {
                case NegocioException negocioException:
                    await SalvaLogAsync(LogNivel.Negocio, context.Exception.Message, internalIP,
                        context.Exception.StackTrace, context.Exception.InnerException?.ToString());
                    context.Result = new ResultadoBaseResult(context.Exception.Message, negocioException.StatusCode);
                    break;
                case ValidacaoException validacaoException:
                    await SalvaLogAsync(LogNivel.Negocio, context.Exception.Message, internalIP,
                        context.Exception.StackTrace,context.Exception.InnerException?.ToString());
                    context.Result = new ResultadoBaseResult(new RetornoBaseDto(validacaoException.Erros));
                    break;
                default:
                    await SalvaLogAsync(LogNivel.Critico, context.Exception.Message, internalIP,
                        context.Exception.StackTrace,context.Exception.InnerException?.ToString());
                    context.Result = new ResultadoBaseResult("Ocorreu um erro interno. Favor contatar o suporte.", 500);
                    break;
            }

            //poderia logar essa exception no logger para entender o que esta acontecendo em caso de erro
            logger.LogError(context.Exception, "");
            base.OnException(context);
        }

        public async Task SalvaLogAsync(LogNivel nivel, string erro, string observacoes, string stackTrace, string innerException)
        {
            await mediator.Send(new SalvarLogViaRabbitCommand(erro, nivel, LogContexto.Geral, observacoes,
                rastreamento: stackTrace,innerException:innerException));
        }
    }
}