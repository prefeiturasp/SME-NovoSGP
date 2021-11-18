using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioOcorrenciasUseCase : AbstractUseCase, IRelatorioOcorrenciasUseCase
    {
        public RelatorioOcorrenciasUseCase(IMediator mediator) : base(mediator){}
        public async Task<bool> Executar(FiltroImpressaoOcorrenciaDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRf = usuarioLogado.CodigoRf;
            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.RelatorioOcorrencias, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosImpressaoOcorrencias));
        }
    }
}
