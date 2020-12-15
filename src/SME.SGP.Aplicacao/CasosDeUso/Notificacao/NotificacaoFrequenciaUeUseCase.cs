using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoFrequenciaUeUseCase : AbstractUseCase, INotificacaoFrequenciaUeUseCase
    {
        public NotificacaoFrequenciaUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var periodoEncerrado = await ObterPeriodoEncerrado();
            if (periodoEncerrado != null)
            {
                await mediator.Send(new NotificaFrequenciaPeriodoUeCommand(periodoEncerrado));
            }

            return true;
        }

        private async Task<PeriodoEscolar> ObterPeriodoEncerrado()
        {
            var ano = DateTime.Now.Year;
            var dataEncerramento = DateTime.Now.Date.AddDays(-1);
            return await mediator.Send(new ObterPeriodoEscolarPorModalidadeAnoEDataFinalQuery(Dominio.ModalidadeTipoCalendario.FundamentalMedio, ano, dataEncerramento));
        }
    }
}
