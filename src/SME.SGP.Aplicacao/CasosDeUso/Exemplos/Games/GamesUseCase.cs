using MediatR;
using SME.SGP.Aplicacao.Commands.Relatorios.GerarRelatorio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
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
        public async Task<bool> Executar(FiltroRelatorioGamesDto filtroRelatorioGamesDto)
        {
            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Games, filtroRelatorioGamesDto));
        }
    }
}
