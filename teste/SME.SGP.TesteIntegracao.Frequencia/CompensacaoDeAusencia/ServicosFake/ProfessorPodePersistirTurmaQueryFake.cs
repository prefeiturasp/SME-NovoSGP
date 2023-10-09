using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia.ServicosFake
{
    public class ProfessorPodePersistirTurmaQueryFake : IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>
    {
        public async Task<bool> Handle(ProfessorPodePersistirTurmaQuery request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
