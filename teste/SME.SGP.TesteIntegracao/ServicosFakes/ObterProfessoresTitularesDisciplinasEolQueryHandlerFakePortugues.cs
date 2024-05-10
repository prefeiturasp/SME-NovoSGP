using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues :
        IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>,
        IRequestHandler<ObterProfessoresTitularesDaTurmaCompletosQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDisciplinasEolQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ProfessorTitularDisciplinaEol>()
            {
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf ="111111",
                    ProfessorNome ="PROFESSOR DE PORTUGUES",
                    DisciplinaNome = "LÍNGUA PORTUGUESA",
                    CodigosDisciplinas = "138"
                },
            });
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDaTurmaCompletosQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ProfessorTitularDisciplinaEol>()
            {
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf ="2222222",
                    ProfessorNome ="PROFESSOR DE PORTUGUES",
                    DisciplinaNome = "LÍNGUA PORTUGUESA",
                    CodigosDisciplinas = "138"
                },
            });
        }
    }
}
