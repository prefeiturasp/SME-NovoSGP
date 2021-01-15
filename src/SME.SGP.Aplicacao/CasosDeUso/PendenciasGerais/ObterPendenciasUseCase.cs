using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<PaginacaoResultadoDto<PendenciaDto>> Executar()
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            return await mediator.Send(new ObterPendenciasPorUsuarioQuery(usuarioId));
        }
    }
}
