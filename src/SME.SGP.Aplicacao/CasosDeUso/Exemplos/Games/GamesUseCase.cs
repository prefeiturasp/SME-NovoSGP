using MediatR;
using SME.SGP.Dominio;
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
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var retorno = await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioExemplo, filtroRelatorioGamesDto, usuarioLogado));

            return retorno;
        }
    }
}
