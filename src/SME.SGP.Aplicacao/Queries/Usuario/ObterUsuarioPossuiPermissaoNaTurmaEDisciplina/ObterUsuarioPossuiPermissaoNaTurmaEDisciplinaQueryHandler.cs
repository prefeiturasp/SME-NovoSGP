using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandler : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>
    {
        private readonly IMediator mediator;

        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await mediator.Send(new PodePersistirTurmaDisciplinaQuery(request.Usuario.CodigoRf, request.CodigoTurma, request.ComponenteCurricularId.ToString(), request.Data.Ticks));
        }
    }
}
