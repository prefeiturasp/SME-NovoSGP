using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioPlanoAnualUseCase : IRelatorioPlanoAnualUseCase
    {
        private readonly IMediator mediator;

        public RelatorioPlanoAnualUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioPlanoAnualDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.Usuario = $"{usuarioLogado.Nome} ({usuarioLogado.ObterCodigoRfLogin()})";

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioPlanoAnual, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosPlanoAnual));
        }
    }
}
