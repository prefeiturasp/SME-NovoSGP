using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase : IListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase
    {
        private readonly IMediator mediator;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(long ano, long componenteCurricularId, bool ensinoEspecial)
        {
            return await mediator.Send(new ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(ano, componenteCurricularId, ensinoEspecial));
        }
    }
}
