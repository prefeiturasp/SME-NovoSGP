using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioLeituraComunicadosUseCase : AbstractUseCase, IRelatorioLeituraComunicadosUseCase
    {
        public RelatorioLeituraComunicadosUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(FiltroRelatorioLeituraComunicados filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.NomeUsuario = usuarioLogado.Nome;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Leitura, filtro, usuarioLogado));
        }

    }
}
