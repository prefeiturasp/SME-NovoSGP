using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObservacaoCartaIntencoesUseCase : IListarObservacaoCartaIntencoesUseCase
    {
        private readonly IMediator mediator;

        public ListarObservacaoCartaIntencoesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ListarObservacaoCartaIntencoesDto>> Executar(long cartaIntencoesId)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new ListarObservacaoCartaIntencoesQuery(cartaIntencoesId, usuarioId));
        }
    }
}
