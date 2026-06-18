using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasUseCase : AbstractUseCase, IObterPendenciasUseCase
    {
        public ObterPendenciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<PendenciaDto>> Executar(string turmaCodigo, int tipoPendencia, string tituloPendencia)
        {
            var usuarioId = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);

            return await mediator.Send(new ObterPendenciasPorUsuarioQuery(usuarioId, turmaCodigo, tipoPendencia, tituloPendencia));
        }
    }
}
