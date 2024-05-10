using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterProfessoresTitularesDaTurmaCompletosQueryHandlerFake : IRequestHandler<ObterProfessoresTitularesDaTurmaCompletosQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        protected const string USUARIO_CP_LOGIN_3333333 = "3333333";

        public Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDaTurmaCompletosQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<ProfessorTitularDisciplinaEol>>(new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol()
                {
                    ProfessorRf = USUARIO_CP_LOGIN_3333333
                }
            });
        }
    }
}
