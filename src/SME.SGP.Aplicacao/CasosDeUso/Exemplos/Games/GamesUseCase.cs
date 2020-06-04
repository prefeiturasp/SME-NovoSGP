using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GamesUseCase : IGamesUseCase
    {
        private readonly IMediator mediator;

        public GamesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task Executar()
        {
            await mediator.Send(new RelatorioGamesCommand());
        }
    }
}
