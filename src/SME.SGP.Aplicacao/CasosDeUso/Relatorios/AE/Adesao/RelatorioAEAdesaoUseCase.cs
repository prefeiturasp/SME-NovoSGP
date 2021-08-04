using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAEAdesaoUseCase : AbstractUseCase, IRelatorioAEAdesaoUseCase
    {
        public RelatorioAEAdesaoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(FiltroRelatorioAEAdesaoDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRF = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AEAdesao, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosEscolaAquiAdesao));
        }
    }
}
