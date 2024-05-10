using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaAnoSyncUseCase : AbstractUseCase, IConciliacaoFrequenciaAnoSyncUseCase
    {
        public ConciliacaoFrequenciaAnoSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<ConciliacaoFrequenciaAnoSyncDto>();
            await mediator.Send(new ConciliacaoFrequenciaAnoCommand(filtro.AnoLetivo));
            return true;
        }
    }
}
