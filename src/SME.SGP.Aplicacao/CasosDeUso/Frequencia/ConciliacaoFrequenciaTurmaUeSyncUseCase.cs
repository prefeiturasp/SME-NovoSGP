using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaUeSyncUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmaUeSyncUseCase
    {
        public ConciliacaoFrequenciaTurmaUeSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<ConciliacaoFrequenciaTurmaUeSyncDto>();
            await mediator.Send(new ConciliacaoFrequenciaTurmaUeCommand(filtro.UeCodigo, filtro.AnoLetivo));
            return true;
        }
    }
}
