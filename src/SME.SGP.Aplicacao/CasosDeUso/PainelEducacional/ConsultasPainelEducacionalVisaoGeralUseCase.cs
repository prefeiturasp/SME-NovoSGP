using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaGlobal;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasPainelEducacionalVisaoGeralUseCase : IConsultasVisaoGeralPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasPainelEducacionalVisaoGeralUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalVisaoGeralRetornoDto>> ObterVisaoGeralConsolidada(int anoLetivo, string codigoDre)
        {
            return await mediator.Send(new PainelEducacionalVisaoGeralQuery(anoLetivo, codigoDre));
        }
    }
}
