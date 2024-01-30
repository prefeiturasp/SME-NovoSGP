using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.DiarioBordo.ServicosFakes
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeDiarioClasse : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
        {
            protected const string COMPONENTE_CURRICULAR_512 = "512";
            protected const long TURMA_ID_1 = 1;
            protected const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
            protected const string USUARIO_PROFESSOR_CODIGO_RF_1111111 = "1111111";

            public ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeDiarioClasse()
            {}

            public Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new ProfessorTitularDisciplinaEol()
                {
                    CodigosDisciplinas = request.ComponenteCurricularCodigo,
                    ProfessorNome = "Teste",
                    ProfessorRf = request.ComponenteCurricularCodigo == COMPONENTE_CURRICULAR_512 ? USUARIO_PROFESSOR_CODIGO_RF_1111111 : USUARIO_PROFESSOR_CODIGO_RF_2222222,
                    TurmaId = TURMA_ID_1
                });
            }
        }
    }
