using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal.ServicosFakes
{
    public class ObterProfessoresTitularesDasTurmasQueryHandlerFake : IRequestHandler<ObterProfessoresTitularesDasTurmasQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private const string CODIGO_RF_1111111 = "1111111";
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDasTurmasQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ProfessorTitularDisciplinaEol>()
            {
                new () { ProfessorRf = CODIGO_RF_1111111 }
            });
        }
    }
}
