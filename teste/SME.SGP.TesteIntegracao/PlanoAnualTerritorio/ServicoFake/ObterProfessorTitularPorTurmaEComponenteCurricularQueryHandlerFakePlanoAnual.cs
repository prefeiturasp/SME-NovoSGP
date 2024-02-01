using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAnualTerritorio.ServicoFake
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakePlanoAnual : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        protected const long COMPONENTE_CURRICULAR_1111 = 1111;
        protected const long TURMA_ID_1 = 1;
        public ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakePlanoAnual()
        { }

        public async Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new ProfessorTitularDisciplinaEol()
            {
                CodigosDisciplinas = COMPONENTE_CURRICULAR_1111.ToString(),
                ProfessorNome = "Não há professor titular",
                ProfessorRf = "",
                TurmaId = TURMA_ID_1
            });
        }
    }
}
