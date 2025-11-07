using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacao;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAprovacaoUseCase : IConsultasAprovacaoUseCase
    {
        private readonly IMediator mediator;

        public ConsultasAprovacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalAprovacaoDto>> ObterAprovacao(int anoLetivo, string codigoDre, string codigoUe)
        {
            return await mediator.Send(new PainelEducacionalAprovacaoQuery(anoLetivo, codigoDre, codigoUe));
        }
    }
}
