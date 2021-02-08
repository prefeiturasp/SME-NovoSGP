using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesBaseUseCase : AbstractUseCase, IObterQuestoesBaseUseCase
    {
        public ObterQuestoesBaseUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ItineranciaQuestoesBaseDto> Executar()
                => await mediator.Send(new ObterQuestoesBaseItineranciaEAlunoQuery());
    }
}
