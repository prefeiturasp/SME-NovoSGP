using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaComponenteSemAula.ServicosFakes
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFake : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        public Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ProfessorTitularDisciplinaEol { ProfessorRf = "1111111", ProfessorNome = "FULANO DE TAL" });
        }
    }
}
