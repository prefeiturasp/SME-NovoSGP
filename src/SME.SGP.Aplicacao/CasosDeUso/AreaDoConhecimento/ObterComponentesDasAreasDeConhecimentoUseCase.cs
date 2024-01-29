using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesDasAreasDeConhecimentoUseCase : AbstractUseCase, IObterComponentesDasAreasDeConhecimentoUseCase
    {
        public ObterComponentesDasAreasDeConhecimentoUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<IEnumerable<DisciplinaDto>> Executar((IEnumerable<DisciplinaDto> Disciplinas, IEnumerable<AreaDoConhecimentoDto> AreasConhecimento) param)
        {
            return await mediator
                .Send(new ObterComponentesAreasConhecimentoQuery(param.Disciplinas, param.AreasConhecimento));
        }
    }
}
