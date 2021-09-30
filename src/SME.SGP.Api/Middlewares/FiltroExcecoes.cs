using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Excecoes;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SME.SGP.Api.Middlewares
{
    public class FiltroExcecoesAttribute : ExceptionFilterAttribute
    {
        private readonly IMediator mediator;

        public FiltroExcecoesAttribute(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public override async void OnException(ExceptionContext context)
        {
            var internalIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList?.Where(c => c.AddressFamily == AddressFamily.InterNetwork).ToString();

            switch (context.Exception)
            {
                case NegocioException negocioException:
                    await SalvaLogAsync(LogNivel.Negocio, context.Exception.Message, internalIP);
                    context.Result = new ResultadoBaseResult(context.Exception.Message, negocioException.StatusCode);
                    break;
                case ValidacaoException validacaoException:
                    await SalvaLogAsync(LogNivel.Negocio, context.Exception.Message, internalIP);
                    context.Result = new ResultadoBaseResult(new RetornoBaseDto(validacaoException.Erros));
                    break;
                default:
                    await SalvaLogAsync(LogNivel.Critico, context.Exception.Message, internalIP);
                    context.Result = new ResultadoBaseResult("Ocorreu um erro interno. Favor contatar o suporte.", 500);
                    break;
            }

            base.OnException(context);
        }
        public async Task SalvaLogAsync(LogNivel nivel, string erro, string observacoes)
        {
            await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Geral, observacoes));
        }
    }
}