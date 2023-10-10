using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal.ServicosFakes
{
    public class ObterProfessoresTitularesDasTurmasQueryHandlerFake : IRequestHandler<ObterProfessoresTitularesDasTurmasQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private const string CODIGO_RF_1111111 = "1111111";
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDasTurmasQuery request, CancellationToken cancellationToken)
        {
            return new List<ProfessorTitularDisciplinaEol>()
            {
                new () { ProfessorRf = CODIGO_RF_1111111 }
            };
        }
    }
}
