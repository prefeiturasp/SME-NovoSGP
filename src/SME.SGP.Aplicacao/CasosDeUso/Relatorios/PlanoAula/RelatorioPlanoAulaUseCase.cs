using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioPlanoAulaUseCase : IRelatorioPlanoAulaUseCase
    {
        private readonly IMediator mediator;

        public RelatorioPlanoAulaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioPlanoAulaDto filtro)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            filtro.Usuario = usuarioLogado;

            if (usuarioLogado.EhNulo())
            {
                throw new NegocioException("Não foi possível identificar o usuário");
            }

            await mediator.Send(new ValidaSeExistePlanoAulaPorIdQuery(filtro.PlanoAulaId));

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.PlanoAula, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosPlanoDeAula));
        }
    }
}
