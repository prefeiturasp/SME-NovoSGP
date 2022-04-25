using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaMesCommandHandler : IRequestHandler<IncluirFilaConciliacaoFrequenciaTurmaMesCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConciliacaoFrequenciaTurmaMesCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConciliacaoFrequenciaTurmaMesCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
