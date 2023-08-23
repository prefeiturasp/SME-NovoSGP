using MediatR;
using SME.SGP.Aplicacao.Interfaces;
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

        public async Task<IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>> Executar((IEnumerable<DisciplinaDto> Disciplinas, IEnumerable<AreaDoConhecimentoDto> AreasConhecimento, 
                                                                                                                       IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto> ComponentesCuricularesGrupoAreaOrdem, long GrupoMatrizId) param)
        {
            return await mediator
                .Send(new MapearAreasConhecimentoQuery(param.Disciplinas, param.AreasConhecimento, param.ComponentesCuricularesGrupoAreaOrdem, param.GrupoMatrizId));
        }
    }
}
