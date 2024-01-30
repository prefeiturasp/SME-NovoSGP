using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.ServicosFake
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
