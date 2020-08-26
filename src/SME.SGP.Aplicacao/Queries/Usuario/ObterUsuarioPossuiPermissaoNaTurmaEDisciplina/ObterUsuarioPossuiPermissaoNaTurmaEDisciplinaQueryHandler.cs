using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandler : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>
    {
        private readonly IServicoEol servicoEOL;

        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }
        public async Task<bool> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await servicoEOL.PodePersistirTurmaDisciplina(request.Usuario.CodigoRf, request.CodigoTurma, request.ComponenteCurricularId.ToString(), request.Data);
        }
    }
}
