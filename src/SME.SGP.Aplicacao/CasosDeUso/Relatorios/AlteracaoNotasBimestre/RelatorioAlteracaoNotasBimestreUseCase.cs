using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Relatorios.AlteracaoNotasBimestre
{
    public class RelatorioAlteracaoNotasBimestreUseCase : AbstractUseCase, IRelatorioAlteracaoNotasBimestreUseCase
    {
        public RelatorioAlteracaoNotasBimestreUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(FiltroRelatorioAlteracaoNotasBimestre filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.NomeUsuario = usuarioLogado.Nome;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.AlteracaoNotasBimestre, filtro, usuarioLogado));
        }
    }
}
