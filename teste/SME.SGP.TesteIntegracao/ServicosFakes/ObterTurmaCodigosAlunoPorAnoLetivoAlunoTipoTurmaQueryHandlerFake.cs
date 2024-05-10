using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake : IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>
    {
        private const string CODIGO_TURMA = "1";
        public async Task<string[]> Handle(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new string[] { CODIGO_TURMA });
        }
    }
}
