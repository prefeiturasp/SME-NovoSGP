using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class IncluirFilaExclusaoAulaRecorrenteCommandHandlerFake : IRequestHandler<IncluirFilaExclusaoAulaRecorrenteCommand, bool>
    {

        private readonly IMediator mediator;

        public IncluirFilaExclusaoAulaRecorrenteCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaExclusaoAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new ExcluirAulaRecorrenteCommand(request.AulaId,
                                                                   request.Recorrencia,
                                                                   request.ComponenteCurricularNome,
                                                                   request.Usuario));

            return true;
        }
    }
}
