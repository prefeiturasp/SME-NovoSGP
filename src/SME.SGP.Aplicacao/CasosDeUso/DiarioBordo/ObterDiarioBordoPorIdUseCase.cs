using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoPorIdUseCase : AbstractUseCase, IObterDiarioBordoPorIdUseCase
    {
        public ObterDiarioBordoPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DiarioBordoDetalhesDto> Executar(long id)
        {
            return await mediator.Send(new ObterDiarioDeBordoPorIdQuery(id));
        }
    }
}
