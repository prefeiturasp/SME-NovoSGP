using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ProfessorPodePersistirTurmaQueryHandlerSemPermissaoFake : IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>
    {
        private readonly IMediator mediator;

        public ProfessorPodePersistirTurmaQueryHandlerSemPermissaoFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ProfessorPodePersistirTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(false);
        }
    }
}