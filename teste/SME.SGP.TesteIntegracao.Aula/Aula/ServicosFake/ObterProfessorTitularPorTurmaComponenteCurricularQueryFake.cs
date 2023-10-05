using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.ServicosFake
{
    public class ObterProfessorTitularPorTurmaComponenteCurricularQueryFake : IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>
    {
        public ObterProfessorTitularPorTurmaComponenteCurricularQueryFake()
        {
        }

        public async Task<ProfessorTitularDisciplinaEol> Handle(ObterProfessorTitularPorTurmaEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            var dadosProfessor = await Task.FromResult(new ProfessorTitularDisciplinaEol
            {
                ProfessorRf = "123456"
            });
            return dadosProfessor;
        }
    }
}
