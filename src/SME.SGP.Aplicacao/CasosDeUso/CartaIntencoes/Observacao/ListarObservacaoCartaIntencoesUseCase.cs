using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
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

            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(param.TurmaCodigo));
            if (turmaId == 0)
                throw new NegocioException("Turma não encontrada.");

            return await mediator.Send(new ListarCartaIntencoesObservacaoQuery(turmaId, param.ComponenteCurricularId, usuarioId));
        }
    }
}
