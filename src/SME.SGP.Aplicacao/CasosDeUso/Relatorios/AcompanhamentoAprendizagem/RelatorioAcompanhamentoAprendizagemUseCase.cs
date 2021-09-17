using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAcompanhamentoAprendizagemUseCase : AbstractUseCase, IRelatorioAcompanhamentoAprendizagemUseCase
    {
        public RelatorioAcompanhamentoAprendizagemUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioAcompanhamentoAprendizagemDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AcompanhamentoAprendizagem, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosRelatorioAcompanhamentoAprendizagem));
        }
    }
}
