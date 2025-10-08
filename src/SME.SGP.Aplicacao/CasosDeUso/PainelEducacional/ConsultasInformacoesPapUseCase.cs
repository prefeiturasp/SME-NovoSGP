using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasInformacoesPapUseCase : IConsultasInformacoesPapUseCase
    {
        private readonly IMediator mediator;

        public ConsultasInformacoesPapUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IndicadoresPapDto> ObterInformacoesPap(int anoLetivo, string codigoDre, string codigoUe)
        {
            return await mediator.Send(new ObterIndicadoresPapQuery(anoLetivo, codigoDre, codigoUe));
        }
    }
}
