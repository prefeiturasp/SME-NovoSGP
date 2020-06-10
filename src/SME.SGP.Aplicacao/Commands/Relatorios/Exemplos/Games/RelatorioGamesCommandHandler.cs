using MediatR;
using SME.SGP.Aplicacao.Commands.Relatorios.GerarRelatorio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioGamesCommandHandler : IRequestHandler<RelatorioGamesCommand, bool>
    {
        private readonly IMediator mediator;

        public RelatorioGamesCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(RelatorioGamesCommand request, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Games, request));
        }
    }
}
