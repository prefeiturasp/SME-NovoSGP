using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
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

        public async Task<IEnumerable<ObjetivoAprendizagem>> Executar(long ano, long componenteCurricularId)
        {
            return await mediator.Send(new ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(ano, componenteCurricularId));
        }
    }
}
