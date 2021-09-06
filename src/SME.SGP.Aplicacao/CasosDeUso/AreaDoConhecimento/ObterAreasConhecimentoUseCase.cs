using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterAreasConhecimento;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.AreaDoConhecimento
{
    public class ObterAreasConhecimentoUseCase : AbstractUseCase, IObterAreasConhecimentoUseCase
    {
        public ObterAreasConhecimentoUseCase(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<IEnumerable<AreaDoConhecimentoDto>> Executar(IEnumerable<DisciplinaDto> param)
        {
            return await mediator
                .Send(new ObterAreasConhecimentoQuery(param));
        }
    }
}
