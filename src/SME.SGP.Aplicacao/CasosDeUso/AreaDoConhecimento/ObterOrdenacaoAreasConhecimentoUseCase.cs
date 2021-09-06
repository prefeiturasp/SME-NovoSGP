using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
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
            return await mediator
                .Send(new ObterOrdenacaoAreasConhecimentoQuery(param.Item1, param.Item2));
        }
    }
}
