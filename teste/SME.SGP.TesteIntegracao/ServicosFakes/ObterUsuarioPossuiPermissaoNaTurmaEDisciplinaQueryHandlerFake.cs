using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerFake : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>
    {
        const long COMPONENTE_CURRICULAR_139 = 139;
        public async Task<bool> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.ComponenteCurricularId != COMPONENTE_CURRICULAR_139);
        }
    }
}
