using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAtribuicaoCJUseCase : IRelatorioAtribuicaoCJUseCase
    {
        private readonly IMediator mediator;

        public RelatorioAtribuicaoCJUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioAtribuicaoCJDto filtro)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtro.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtro.UeCodigo));
            if (!string.IsNullOrEmpty(filtro.UsuarioRf))
            {
                var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(filtro.UsuarioRf));
                if (usuario == null)
                    throw new NegocioException($@"Não existe usuário com o código RF {filtro.UsuarioRf}");
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AtribuicaoCJ, filtro, usuarioLogado));
        }
    }
}
