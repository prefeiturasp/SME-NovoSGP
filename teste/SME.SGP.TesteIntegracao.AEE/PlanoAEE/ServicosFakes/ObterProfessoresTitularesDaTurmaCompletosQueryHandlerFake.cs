using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterProfessoresTitularesDaTurmaCompletosQueryHandlerFake : IRequestHandler<ObterProfessoresTitularesDaTurmaCompletosQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        protected const string USUARIO_CP_LOGIN_3333333 = "3333333";

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDaTurmaCompletosQuery request, CancellationToken cancellationToken)
        {
            return new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol()
                {
                    ProfessorRf = USUARIO_CP_LOGIN_3333333
                }
            };
        }
    }
}
