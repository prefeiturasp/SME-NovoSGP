using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes
{
    public class ObterConselhoClasseConsolidadoPorTurmaAlunoQueryHandlerFake : IRequestHandler<ObterConselhoClasseConsolidadoPorTurmaAlunoQuery, ConselhoClasseConsolidadoTurmaAluno>
    {
        public ObterConselhoClasseConsolidadoPorTurmaAlunoQueryHandlerFake()
        {
        }

        public async Task<ConselhoClasseConsolidadoTurmaAluno> Handle(ObterConselhoClasseConsolidadoPorTurmaAlunoQuery request, CancellationToken cancellationToken)
        {
            return new ConselhoClasseConsolidadoTurmaAluno() { Id = 1, Status = 0, AlunoCodigo = "1", TurmaId = 1, ParecerConclusivoId = null, CriadoRF="1",CriadoPor = "1",CriadoEm = DateTime.Now};
        }
    }
}
