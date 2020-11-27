using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoAndamentoFechamentoUseCase : AbstractUseCase, IExecutaNotificacaoAndamentoFechamentoUseCase
    {
        public ExecutaNotificacaoAndamentoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            var anoAtual = DateTime.Now.Year;
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasNotificacaoAndamentoFechamento, anoAtual));

            foreach(var parametro in parametros)
            {
                var diasParaEncerramento = int.Parse(parametro.Valor);
                await VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario.FundamentalMedio, diasParaEncerramento);
                await VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario.EJA, diasParaEncerramento);
            }
        }

        private async Task VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario modalidade, int diasParaEncerramento)
        {
            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoBimestrePorDataFinalQuery(modalidade, DateTime.Now.Date.AddDays(diasParaEncerramento)));
            foreach(var periodoEncerrando in periodosEncerrando)
            {
                await mediator.Send(new ExecutaNotificacaoAndamentoFechamentoCommand(periodoEncerrando, modalidade));
            }
        }
    }
}
