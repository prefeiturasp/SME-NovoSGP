using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class VerificaNotasTodosComponentesCurricularesQueryHandlerFake : IRequestHandler<VerificaNotasTodosComponentesCurricularesQuery, bool>
    {
        private const string TURMA_CODIGO_1 = "1";
        private const string ALUNO_CODIGO_1 = "1";

        public VerificaNotasTodosComponentesCurricularesQueryHandlerFake()
        {}

        public async Task<bool> Handle(VerificaNotasTodosComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            if (request.Turma.CodigoTurma.Equals(TURMA_CODIGO_1)
                && request.AlunoCodigo.Equals(ALUNO_CODIGO_1))
                return true;
            return false;
        }
    }
}