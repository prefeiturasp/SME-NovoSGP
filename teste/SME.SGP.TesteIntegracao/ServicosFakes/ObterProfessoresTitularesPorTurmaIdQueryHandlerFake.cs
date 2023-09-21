﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFake
{
    public class ObterProfessoresTitularesPorTurmaIdQueryHandlerFake : IRequestHandler<ObterProfessoresTitularesPorTurmaIdQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            return new List<ProfessorTitularDisciplinaEol>()
            {
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf ="222222",
                    ProfessorNome ="PROFESSOR DE PORTUGUES",
                    DisciplinaNome = "LÍNGUA PORTUGUESA",
                    DisciplinasId = new long[] { 138 }
                },
            };
        }
    }
}
