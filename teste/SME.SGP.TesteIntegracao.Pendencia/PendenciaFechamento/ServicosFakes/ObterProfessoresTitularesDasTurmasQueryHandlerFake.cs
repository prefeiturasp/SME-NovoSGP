using MediatR;
using SME.SGP.Aplicacao;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento.ServicosFakes
{
    public class ObterProfessoresTitularesDasTurmasQueryHandlerFake : IRequestHandler<ObterProfessoresTitularesDasTurmasQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDasTurmasQuery request, CancellationToken cancellationToken)
        {
            return new List<ProfessorTitularDisciplinaEol>()
            {
                new () { ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222 }
            };
        }
    }
}
