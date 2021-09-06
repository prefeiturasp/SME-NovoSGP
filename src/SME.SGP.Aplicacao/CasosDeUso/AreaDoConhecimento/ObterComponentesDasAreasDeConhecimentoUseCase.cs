using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao;
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

        public async Task<IEnumerable<DisciplinaDto>> Executar((IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>) param)
        {
            return await mediator
                .Send(new ObterComponentesAreasConhecimentoQuery(param.Item1, param.Item2));
        }
    }
}
