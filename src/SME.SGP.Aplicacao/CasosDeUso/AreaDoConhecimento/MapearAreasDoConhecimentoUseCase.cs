using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Aplicacao.Queries.AreaDoConhecimento.MapearAreasConhecimento;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class MapearAreasDoConhecimentoUseCase : AbstractUseCase, IMapearAreasDoConhecimentoUseCase
    {
        public MapearAreasDoConhecimentoUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>> Executar((IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>, long) param)
        {
            return await mediator
                .Send(new MapearAreasConhecimentoQuery(param.Item1, param.Item2, param.Item3, param.Item4));
        }
    }
}
