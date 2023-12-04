using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.DiarioBordo.ServicosFakes
{
    internal class ObterProfessoresTitularesDasTurmasQueryHandlerFakeCartaIntencoes: IRequestHandler<ObterProfessoresTitularesDasTurmasQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private const string USUARIO_PROFESSOR_CODIGO_RF_9999999 = "9999999";
        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDasTurmasQuery request, CancellationToken cancellationToken)
        {
            return new List<ProfessorTitularDisciplinaEol>()
            {
                new () { ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_9999999 }
            };
        }
    }
}
