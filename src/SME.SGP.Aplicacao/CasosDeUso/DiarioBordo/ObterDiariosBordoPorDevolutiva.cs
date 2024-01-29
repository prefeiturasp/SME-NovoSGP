using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosBordoPorDevolutiva : AbstractUseCase, IObterDiariosBordoPorDevolutiva
    {
        public ObterDiariosBordoPorDevolutiva(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> Executar(long devolutivaId, int anoLetivo)
            => await mediator.Send(new ObterDiariosBordoPorDevolutivaQuery(devolutivaId, anoLetivo));
    }
}
