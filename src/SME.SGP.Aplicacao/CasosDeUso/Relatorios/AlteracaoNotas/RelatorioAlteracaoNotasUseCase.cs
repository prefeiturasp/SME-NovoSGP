using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioAlteracaoNotasUseCase : AbstractUseCase, IRelatorioAlteracaoNotasUseCase
    {
        public RelatorioAlteracaoNotasUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(FiltroRelatorioAlteracaoNotas filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.NomeUsuario = usuarioLogado.Nome;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AlteracaoNotasBimestre, filtro, usuarioLogado));
        }
    }
}
