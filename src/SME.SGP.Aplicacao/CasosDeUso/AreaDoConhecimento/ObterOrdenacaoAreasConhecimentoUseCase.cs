using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOrdenacaoAreasConhecimentoUseCase : AbstractUseCase, IObterOrdenacaoAreasConhecimentoUseCase
    {
        public ObterOrdenacaoAreasConhecimentoUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>> Executar((IEnumerable<DisciplinaDto> Disciplinas, IEnumerable<AreaDoConhecimentoDto> AreasConhecimento) param)
        {
            return await mediator
                .Send(new ObterOrdenacaoAreasConhecimentoQuery(param.Disciplinas, param.AreasConhecimento));
        }
    }
}
