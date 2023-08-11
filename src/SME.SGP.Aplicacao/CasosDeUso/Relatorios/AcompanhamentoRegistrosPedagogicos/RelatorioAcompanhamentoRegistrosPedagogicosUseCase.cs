using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAcompanhamentoRegistrosPedagogicosUseCase : AbstractUseCase, IRelatorioAcompanhamentoRegistrosPedagogicosUseCase
    {
        public RelatorioAcompanhamentoRegistrosPedagogicosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioAcompanhamentoRegistrosPedagogicosDto filtro)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRF = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AcompanhamentoRegistrosPedagogicos, filtro, usuarioLogado,
                rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosAcompanhamentoRegistrosPedagogicos));
        }
    }
}
