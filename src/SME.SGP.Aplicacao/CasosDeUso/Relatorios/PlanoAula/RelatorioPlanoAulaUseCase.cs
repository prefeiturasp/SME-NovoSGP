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
    public class RelatorioPlanoAulaUseCase : IRelatorioPlanoAulaUseCase
    {
        private readonly IMediator mediator;

        public RelatorioPlanoAulaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioPlanoAulaDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.Usuario = usuarioLogado;

            if (usuarioLogado == null)
            {
                throw new NegocioException("Não foi possível identificar o usuário");
            }

            await mediator.Send(new ValidaSeExistePlanoAulaPorIdQuery(filtro.PlanoAulaId));

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.PlanoAula, filtro, usuarioLogado));
        }
    }
}
