using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioPlanejamentoAnualUseCase : IRelatorioPlanejamentoAnualUseCase
    {
        private readonly IMediator mediator;

        public RelatorioPlanejamentoAnualUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioPlanejamentoAnualDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.Usuario = $"{usuarioLogado.Nome} ({usuarioLogado.ObterCodigoRfLogin()})";

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioPlanejamentoAnual, filtro, usuarioLogado,rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosPlanejamentoAnual));
        }
    }
}
