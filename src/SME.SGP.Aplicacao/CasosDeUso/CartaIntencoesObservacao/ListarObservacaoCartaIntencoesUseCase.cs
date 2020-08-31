using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase : IListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase
    {
        private readonly IMediator mediator;

        public ListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<CartaIntencoesObservacaoDto>> Executar(BuscaCartaIntencoesObservacaoDto param)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new ListarCartaIntencoesObservacaoQuery(param.TurmaId, param.ComponenteCurricularId, usuarioId));
        }
    }
}
