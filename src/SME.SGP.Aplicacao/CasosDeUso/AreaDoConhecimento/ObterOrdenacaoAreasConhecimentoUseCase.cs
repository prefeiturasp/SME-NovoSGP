using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.AreaDoConhecimento
{
    public class ObterOrdenacaoAreasConhecimentoUseCase : AbstractUseCase, IObterOrdenacaoAreasConhecimentoUseCase
    {
        public ObterOrdenacaoAreasConhecimentoUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>> Executar((IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>) param)
        {
            var listaGrupoMatrizId = param.Item1?
                .Select(a => a.GrupoMatrizId)?.Distinct().ToArray();

            var listaAreaConhecimentoId = param.Item2?
                .Select(a => a.Id).ToArray();

            return await mediator
                .Send(new ObterComponenteCurricularGrupoAreaOrdenacaoQuery(listaGrupoMatrizId, listaAreaConhecimentoId));
        }
    }
}
