using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasUseCase : AbstractUseCase, IObterPendenciasUseCase
    {
        public ObterPendenciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<PendenciaDto>> Executar(string turmaCodigo, int tipoPendencia, string tituloPendencia)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            return await mediator.Send(new ObterPendenciasPorUsuarioQuery(usuarioId, turmaCodigo, tipoPendencia, tituloPendencia));
        }
    }
}
