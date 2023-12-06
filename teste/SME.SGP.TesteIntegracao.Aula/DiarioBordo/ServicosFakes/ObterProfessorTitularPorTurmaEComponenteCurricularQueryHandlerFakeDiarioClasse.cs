using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.DiarioBordo.ServicosFakes
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeDiarioClasse : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        protected const long COMPONENTE_CURRICULAR_512 = 512;
        protected const long TURMA_ID_1 = 1;
        public ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeDiarioClasse()
        { }

        public async Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ProfessorTitularDisciplinaEol()
            {
                DisciplinasId = new long[] { COMPONENTE_CURRICULAR_512 },
                ProfessorNome = "Teste",
                ProfessorRf = "9999999",
                TurmaId = TURMA_ID_1
            });
        }
    }
}
