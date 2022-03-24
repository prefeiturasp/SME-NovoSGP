using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand : IRequest<bool>
    {
        public long ConsolidacaoId { get; set; }
        public IEnumerable<FechamentoNotaMigracaoDto> AlunoNotas { get; set; }
    }
}
