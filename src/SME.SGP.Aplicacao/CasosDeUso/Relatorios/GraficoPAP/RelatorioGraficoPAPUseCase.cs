using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public  class RelatorioGraficoPAPUseCase : IRelatorioGraficoPAPUseCase
    {
        private readonly IMediator mediator;

        public RelatorioGraficoPAPUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioResumoPAPDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRf = usuarioLogado.CodigoRf;

            if (usuarioLogado == null)
            {
                throw new NegocioException("Não foi possível identificar o usuário");
            }

            if (!string.IsNullOrEmpty(filtro.TurmaId) && filtro.TurmaId != "0")
                await mediator.Send(new ValidaSeExisteTurmaPorCodigoQuery(filtro.TurmaId));

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.GraficoPAP, filtro, usuarioLogado));
        }
    }
}
