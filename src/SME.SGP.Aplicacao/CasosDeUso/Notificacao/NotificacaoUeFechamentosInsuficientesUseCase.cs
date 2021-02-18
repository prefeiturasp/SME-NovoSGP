using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoUeFechamentosInsuficientesUseCase : AbstractUseCase, INotificacaoUeFechamentosInsuficientesUseCase
    {
        public NotificacaoUeFechamentosInsuficientesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoAtual = DateTime.Now.Year;
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema.DiasNotificacaoFechamentoPendente, anoAtual));
            var percentualFechamentoInsuficiente = await ObterPercentualFechamentoInsuficienteParaNotificar(anoAtual);

            foreach (var parametro in parametros)
            {
                var diasParaEncerramento = int.Parse(parametro.Valor);
                await VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario.FundamentalMedio, diasParaEncerramento, percentualFechamentoInsuficiente);
                await VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario.EJA, diasParaEncerramento, percentualFechamentoInsuficiente);
            }

            return true;
        }

        private async Task<double> ObterPercentualFechamentoInsuficienteParaNotificar(int anoAtual)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFechamentosInsuficientesNotificacao, anoAtual));

            if (parametro == null)
                throw new NegocioException("Parâmetro de percentual de fechamentos insuficientes para notificação da UE não cadastrado.");

            return double.Parse(parametro.Valor);
        }

        private async Task VerificaPeriodosFechamentoEncerrando(ModalidadeTipoCalendario modalidade, int diasParaEncerramento, double percentualFechamentoInsuficiente)
        {
            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoBimestrePorDataFinalQuery(modalidade, DateTime.Now.Date.AddDays(diasParaEncerramento)));

            var grupoPeriodosDre = periodosEncerrando.GroupBy(a => a.PeriodoFechamento.Ue.DreId);
            foreach(var periodosDre in grupoPeriodosDre)
                await mediator.Send(new ExecutaNotificacaoUeFechamentoInsuficientesCommand(periodosDre, modalidade, percentualFechamentoInsuficiente));
        }
    }
}
