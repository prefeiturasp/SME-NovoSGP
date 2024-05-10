using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesItineranciaAlunoUseCase : AbstractUseCase, IObterQuestoesItineranciaAlunoUseCase
    {
        public ObterQuestoesItineranciaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ItineranciaAlunoQuestaoDto>> Executar(long id)
                => await mediator.Send(new ObterQuestoesItineranciaAlunoPorIdQuery(id));
    }
}
