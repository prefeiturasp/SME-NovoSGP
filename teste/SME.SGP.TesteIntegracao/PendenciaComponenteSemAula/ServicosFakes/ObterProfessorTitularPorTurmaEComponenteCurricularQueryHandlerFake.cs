using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaComponenteSemAula.ServicosFakes
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFake : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        public async Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            if (request.TurmaCodigo == "3")
                return null;
            else
                return new ProfessorTitularDisciplinaEol { ProfessorRf = "1111111", ProfessorNome = "FULANO DE TAL" };
        }
    }
}
