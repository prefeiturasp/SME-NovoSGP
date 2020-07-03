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
        private readonly IUnitOfWork unitOfWork;

        public GamesUseCase(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<bool> Executar(FiltroRelatorioGamesDto filtroRelatorioGamesDto)
        {
            unitOfWork.IniciarTransacao();
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var retorno = await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioExemplo, filtroRelatorioGamesDto, usuarioLogado));
            unitOfWork.PersistirTransacao();
            return retorno;
        }
    }
}
