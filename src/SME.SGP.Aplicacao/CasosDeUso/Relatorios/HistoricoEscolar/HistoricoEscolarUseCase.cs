using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class HistoricoEscolarUseCase : IHistoricoEscolarUseCase
    {
        private readonly IMediator mediator;

        public HistoricoEscolarUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroHistoricoEscolarDto filtroHistoricoEscolarDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroHistoricoEscolarDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroHistoricoEscolarDto.UeCodigo));
            await mediator.Send(new ValidaSeExisteTurmaPorCodigoQuery(filtroHistoricoEscolarDto.TurmaCodigo));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtroHistoricoEscolarDto.Usuario = usuarioLogado ?? throw new NegocioException("Não foi possível localizar o usuário.");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.HistoricoEscolarFundamental, filtroHistoricoEscolarDto, usuarioLogado));

        }
    }
}
