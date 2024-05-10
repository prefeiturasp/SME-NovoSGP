using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao
{
    public class RelatorioEncaminhamentoAeeDetalhadoUseCase : IRelatorioEncaminhamentoAeeDetalhadoUseCase
    {
        private readonly IMediator mediator;

        public RelatorioEncaminhamentoAeeDetalhadoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioEncaminhamentoAeeDetalhadoDto filtro)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuarioLogado.EhNulo())
                throw new NegocioException(
                    "Não foi possível localizar o usuário para realizar a impressão do Encaminhamento AEE.");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioEncaminhamentoAeeDetalhado, filtro, usuarioLogado,
                rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosEncaminhamentoAeeDetalhado));
        }
    }
}