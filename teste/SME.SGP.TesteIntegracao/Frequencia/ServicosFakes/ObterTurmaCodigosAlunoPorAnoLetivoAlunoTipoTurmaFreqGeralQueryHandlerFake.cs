using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFakes
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaFreqGeralQueryHandlerFake : IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>
    {
        private string CODIGO_TURMA_1 = "1";
        private string CODIGO_TURMA_2 = "2";
        public async Task<string[]> Handle(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery request, CancellationToken cancellationToken)
        {
            return new string[] { CODIGO_TURMA_1, CODIGO_TURMA_2 };
        }
    }
}
