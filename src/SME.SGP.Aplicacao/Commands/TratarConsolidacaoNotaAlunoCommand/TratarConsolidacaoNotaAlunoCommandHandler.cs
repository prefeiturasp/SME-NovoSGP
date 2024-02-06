using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarConsolidacaoNotaAlunoCommandHandler : IRequestHandler<TratarConsolidacaoNotaAlunoCommand, bool>
    {
        private readonly IMediator mediator;

        public TratarConsolidacaoNotaAlunoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(TratarConsolidacaoNotaAlunoCommand request, CancellationToken cancellationToken)
        {
            var consolidacaoNotaAlunoDto = request.ConsolidacaoNotaAlunoDto;
            if (consolidacaoNotaAlunoDto.Nota.NaoEhNulo() || consolidacaoNotaAlunoDto.ConceitoId.NaoEhNulo())
                await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAlunoDto), cancellationToken);
            return true;
        }
    }
}
