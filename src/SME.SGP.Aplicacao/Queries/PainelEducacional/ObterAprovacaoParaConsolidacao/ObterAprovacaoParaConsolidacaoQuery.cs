using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoParaConsolidacao
{
    public class ObterAprovacaoParaConsolidacaoQuery : IRequest<IEnumerable<ConsolidacaoAprovacaoDto>>
    {
        public ObterAprovacaoParaConsolidacaoQuery(long[] turmaId)
        {
            TurmaId = turmaId;
        }

        public long[] TurmaId { get; set; }
    }
}
