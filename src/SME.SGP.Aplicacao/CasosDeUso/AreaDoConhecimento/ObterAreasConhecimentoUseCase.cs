using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
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
            var listaCodigosComponentes = param
                .Where(cc => !cc.Regencia)
                .Select(a => a.CodigoComponenteCurricular);

            return await mediator
                .Send(new ObterAreasConhecimentoComponenteCurricularQuery(listaCodigosComponentes.Distinct().ToArray()));
        }
    }
}
