using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CartaIntencoes.ServicosFakes
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeCartaIntencoes : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        protected const string COMPONENTE_CURRICULAR_512 = "512";
        protected const long TURMA_ID_1 = 1;
        public ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeCartaIntencoes()
        { }

        public async Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            return new ProfessorTitularDisciplinaEol()
            {
                CodigosDisciplinas = COMPONENTE_CURRICULAR_512,
                ProfessorNome = "Teste",
                ProfessorRf = "9999999",
                TurmaId = TURMA_ID_1
            };
        }
    }
}
