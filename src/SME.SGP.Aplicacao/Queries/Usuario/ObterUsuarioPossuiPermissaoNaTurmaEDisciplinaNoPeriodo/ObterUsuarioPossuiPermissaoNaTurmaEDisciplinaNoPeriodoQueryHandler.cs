using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQueryHandler : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery, bool>
    {
        private readonly IMediator mediator;

        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaNoPeriodoQuery request, CancellationToken cancellationToken)
            => await mediator.Send(new PodePersistirTurmaNoPeriodoQuery(request.UsuarioRf, request.CodigoTurma, request.ComponenteCurricularId, request.DataInicio, request.DataFim));
    }
}
