using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes
{
    public class SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommandHandlerFake : IRequestHandler<SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand, long>
    {
        public SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommandHandlerFake()
        {
        }

        public async Task<long> Handle(SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand request, CancellationToken cancellationToken)
        {
            return 1;
        }
    }
}
