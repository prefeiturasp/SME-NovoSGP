using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.FechamentoAcompanhamentoTurmas.ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnoAnterior
{
    public class ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand : IRequest<bool>
    {
        public long ConsolidacaoId { get; set; }
        public IEnumerable<FechamentoNotaMigracaoDto> AlunoNotas { get; set; }
    }
}
