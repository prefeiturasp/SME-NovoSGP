using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
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

        public async Task<IEnumerable<PainelEducacionalAprovacaoUeDto>> ObterAprovacao(int anoLetivo, string codigoUe)
        {
            return await mediator.Send(new PainelEducacionalAprovacaoUeQuery(anoLetivo, codigoUe));
        }
    }
}
