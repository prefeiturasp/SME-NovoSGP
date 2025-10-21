using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterReclassificacao;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasReclassificacaoPainelEducacionalUseCase : IConsultasReclassificacaoPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasReclassificacaoPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalReclassificacaoDto>> ObterReclassificacao(string codigoDre, string codigoUe, int anoLetivo, string anoTurma)
        {
            return await mediator.Send(new ObterReclassificacaoQuery(codigoDre, codigoUe, anoLetivo, anoTurma));
        }
    }
}
