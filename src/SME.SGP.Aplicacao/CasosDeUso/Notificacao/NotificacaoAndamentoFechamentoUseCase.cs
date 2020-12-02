using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoAndamentoFechamentoUseCase : AbstractUseCase, INotificacaoAndamentoFechamentoUseCase
    {
        public NotificacaoAndamentoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoAtual = DateTime.Now.Year;
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasNotificacaoAndamentoFechamento, anoAtual));

            foreach (var parametro in parametros)
            {
                var diasParaEncerramento = int.Parse(parametro.Valor);
                await VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario.FundamentalMedio, diasParaEncerramento);
                await VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario.EJA, diasParaEncerramento);
            }

            return true;
        }

        private async Task VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario modalidade, int diasParaEncerramento)
        {
            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoBimestrePorDataFinalQuery(modalidade, DateTime.Now.Date.AddDays(diasParaEncerramento)));
            foreach (var periodoEncerrando in periodosEncerrando)
            {
                await mediator.Send(new ExecutaNotificacaoAndamentoFechamentoCommand(periodoEncerrando, modalidade));
            }
        }
    }
}
