using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaRanking;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasRegistroFrequenciaAgrupamentoRankingUseCase : IConsultasRegistroFrequenciaAgrupamentoRankingUseCase
    {
        private readonly IMediator mediator;

        public ConsultasRegistroFrequenciaAgrupamentoRankingUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<PainelEducacionalRegistroFrequenciaRankingDto> ObterFrequencia(int anoLetivo, string codigoDre, string codigoUe)
        {
            return await mediator.Send(new PainelEducacionalRegistroFrequenciaRakingQuery(anoLetivo, codigoDre, codigoUe));
        }
    }
}
