using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;

namespace SME.SGP.Aplicacao
{
    public class ImpressaoPlanoAeeUseCase : IImpressaoPlanoAeeUseCase
    {
        private readonly IMediator mediator;

        public ImpressaoPlanoAeeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioPlanoAeeDto filtro)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuarioLogado.EhNulo())
                throw new NegocioException(
                    "Não foi possível localizar o usuário para realizar a impressão do Plano AEE.");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.PlanoAee, filtro, usuarioLogado,
                rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosPlanoAee));
        }
    }
}