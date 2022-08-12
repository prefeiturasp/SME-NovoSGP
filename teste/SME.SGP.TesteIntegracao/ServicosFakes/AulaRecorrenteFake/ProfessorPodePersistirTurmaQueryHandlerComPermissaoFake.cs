using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake : IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>
    {
        private readonly IMediator mediator;

        public ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ProfessorPodePersistirTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}