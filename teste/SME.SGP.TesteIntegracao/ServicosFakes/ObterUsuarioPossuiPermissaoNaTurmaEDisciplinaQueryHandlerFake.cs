using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    internal class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerFake : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>
    {
        const long COMPONENTE_CURRICULAR_139 = 139;
        public async Task<bool> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return request.ComponenteCurricularId == COMPONENTE_CURRICULAR_139 ? false : true;
        }
    }
}
