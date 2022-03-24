using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.FechamentoAcompanhamentoTurmas.ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnoAnterior
{
    public class ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommandHandler : IRequestHandler<ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand, bool>
    {
        public Task<bool> Handle(ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
