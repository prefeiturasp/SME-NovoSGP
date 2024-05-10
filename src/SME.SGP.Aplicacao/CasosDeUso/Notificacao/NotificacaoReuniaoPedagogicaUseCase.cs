using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoReuniaoPedagogicaUseCase : AbstractUseCase, INotificacaoReuniaoPedagogicaUseCase
    {
        public NotificacaoReuniaoPedagogicaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoAtual = DateTime.Now.Year;
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasNotificacaoReuniaoPedagogica, anoAtual));

            foreach (var parametro in parametros)
            {
                var diasParaEvento = int.Parse(parametro.Valor);
                await VerificaEventos(TipoEvento.ReuniaoPedagogica, diasParaEvento);
            }

            return true;
        }

        private async Task VerificaEventos(TipoEvento tipoEvento, int diasParaEvento)
        {
            var dataEvento = DateTime.Now.Date.AddDays(diasParaEvento);
            var eventos = await mediator.Send(new ObterEventoPorTipoEDataQuery(tipoEvento, dataEvento));
            foreach(var evento in eventos)
            {
                await mediator.Send(new NotificarEventoCommand(evento));
            }
        }
    }
}
