using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoInicioFimPeriodoFechamentoUseCase : AbstractUseCase, INotificacaoInicioFimPeriodoFechamentoUseCase
    {
        public NotificacaoInicioFimPeriodoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoAtual = DateTime.Now.Year;
            var parametroInicio = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasNotificacaoPeriodoFechamentoInicio, anoAtual));
            var parametroFim = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasNotificacaoPeriodoFechamentoFim, anoAtual));

            await VerificaPeriodosFechamento(ModalidadeTipoCalendario.FundamentalMedio, int.Parse(parametroInicio.Valor), int.Parse(parametroFim.Valor));
            await VerificaPeriodosFechamento(ModalidadeTipoCalendario.EJA, int.Parse(parametroInicio.Valor), int.Parse(parametroFim.Valor));
            return true;
        }

        private async Task VerificaPeriodosFechamento(ModalidadeTipoCalendario modalidade, int diasInicio, int diasFim)
        {
            var periodosIniciando = await mediator.Send(new ObterPeriodosFechamentoBimestrePorDataInicioQuery(modalidade, DateTime.Now.Date.AddDays(diasInicio)));
            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoBimestrePorDataFinalQuery(modalidade, DateTime.Now.Date.AddDays(diasFim)));
            var ues = await mediator.Send(new ObterUesComDrePorModalidadeTurmasQuery(modalidade.ObterModalidades(), DateTime.Now.Year));

            foreach (var periodoIniciando in periodosIniciando)
            {
                foreach (var ue in ues)
                {
                    periodoIniciando.PeriodoFechamento.Ue = ue;
                    await mediator.Send(new ExecutaNotificacaoPeriodoFechamentoIniciandoCommand(periodoIniciando, modalidade));
                }
            }

            foreach (var periodoEncerrando in periodosEncerrando)
            {
                foreach (var ue in ues)
                {
                    periodoEncerrando.PeriodoFechamento.Ue = ue;
                    await mediator.Send(new ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand(periodoEncerrando, modalidade));
                }
            }
        }
    }
}