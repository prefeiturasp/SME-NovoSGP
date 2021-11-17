using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAcompanhamentoDeFrequênciaUseCase : AbstractUseCase, IRelatorioAcompanhamentoDeFrequênciaUseCase
    {
        public RelatorioAcompanhamentoDeFrequênciaUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(FiltroAcompanhamentoFrequenciaJustificativaDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRF = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AcompanhamentoFrequencia, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosAcompanhamentoFrequencia));
        }
    }
}
