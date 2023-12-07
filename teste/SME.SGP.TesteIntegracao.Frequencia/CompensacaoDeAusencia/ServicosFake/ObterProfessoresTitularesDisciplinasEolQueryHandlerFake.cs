using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia.ServicosFake
{
    public class ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues : IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDisciplinasEolQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ProfessorTitularDisciplinaEol>()
            {
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf ="222222",
                    ProfessorNome ="PROFESSOR DE PORTUGUES",
                    DisciplinaNome = "LÍNGUA PORTUGUESA",
                    DisciplinasId = new long[] { 138 }
                },
            });
        }
    }
}
