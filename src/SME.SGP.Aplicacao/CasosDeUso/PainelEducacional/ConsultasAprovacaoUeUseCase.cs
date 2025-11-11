using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAprovacaoUeUseCase : IConsultasAprovacaoUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasAprovacaoUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>> ObterAprovacao(int anoLetivo, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros)
        {
            return await mediator.Send(new PainelEducacionalAprovacaoUeQuery(anoLetivo, codigoUe, modalidade, numeroPagina, numeroRegistros));
        }
    }
}
