using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoPeriodoFechamentoUseCase : AbstractUseCase, INotificacaoPeriodoFechamentoUseCase
    {
        public NotificacaoPeriodoFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var ano = DateTime.Now.Year;

            await VerificaNotificacaoPeriodoFechamentoUe(ano);
            await VerificaNotificacaoPeriodoFechamentoDre(ano);

            return true;
        }

        private async Task VerificaNotificacaoPeriodoFechamentoDre(int ano)
        {
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasNotificacaoPeriodoFechamentoDre, ano));
            foreach (var parametro in parametros)
            {
                var diasParaFechamento = int.Parse(parametro.Valor);

                var periodoIniciando = await ObterPeriodosFechamento(ModalidadeTipoCalendario.FundamentalMedio, DateTime.Now.Date.AddDays(diasParaFechamento));
                if (periodoIniciando != null)
                    await NotificarDre(ModalidadeTipoCalendario.FundamentalMedio, periodoIniciando);

                periodoIniciando = await ObterPeriodosFechamento(ModalidadeTipoCalendario.EJA, DateTime.Now.Date.AddDays(diasParaFechamento));
                if (periodoIniciando != null)
                    await NotificarDre(ModalidadeTipoCalendario.EJA, periodoIniciando);
            }
        }

        private async Task NotificarDre(ModalidadeTipoCalendario modalidade, PeriodoFechamentoBimestre periodoIniciando)
        {
            await mediator.Send(new NotificarPeriodoFechamentoDreCommand(modalidade, periodoIniciando));
        }
        private async Task VerificaNotificacaoPeriodoFechamentoUe(int ano)
        {
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasNotificacaoPeriodoFechamentoUe, ano));
            foreach(var parametro in parametros)
            {
                var diasParaFechamento = int.Parse(parametro.Valor);

                var periodoIniciando = await ObterPeriodosFechamento(ModalidadeTipoCalendario.FundamentalMedio, DateTime.Now.Date.AddDays(diasParaFechamento));
                if (periodoIniciando != null)
                    await NotificarUe(ModalidadeTipoCalendario.FundamentalMedio, periodoIniciando);

                periodoIniciando = await ObterPeriodosFechamento(ModalidadeTipoCalendario.EJA, DateTime.Now.Date.AddDays(diasParaFechamento));
                if (periodoIniciando != null)
                    await NotificarUe(ModalidadeTipoCalendario.EJA, periodoIniciando);
            }
        }

        private async Task NotificarUe(ModalidadeTipoCalendario modalidade, PeriodoFechamentoBimestre periodoIniciando)
        {
            await mediator.Send(new NotificarPeriodoFechamentoUeCommand(modalidade, periodoIniciando));
        }

        private async Task<PeriodoFechamentoBimestre> ObterPeriodosFechamento(ModalidadeTipoCalendario modalidade, DateTime dataInicio)
            => await mediator.Send(new ObterPeriodoFechamentoBimestrePorDreUeEDataQuery(modalidade, dataInicio, 1));
    }
}
