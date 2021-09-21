using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDetalhamentoPendenciaAulaUseCase : AbstractUseCase, IObterDetalhamentoPendenciaAulaUseCase
    {
        public ObterDetalhamentoPendenciaAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DetalhamentoPendenciaAulaDto> Executar(long param)
        {
            var detalhamentoPendenciaAula = await mediator.Send(new ObterDetalhamentoPendenciaAulaQuery(param));
            return detalhamentoPendenciaAula;
        }
    }
}
