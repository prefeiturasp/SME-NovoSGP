using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio;
using SME.SGP.Dominio.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public class ObterSolicitacaoRelatorioUseCase : AbstractUseCase, IObterSolicitacaoRelatorioUseCase
    {
        public ObterSolicitacaoRelatorioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<long> Executar(FiltroSolicitacaoRelatorioDto filtroRelatorio)
        {
            var relatorio = await mediator.Send(new ObterSolicitacaoRelatorioQuery(filtroRelatorio.TipoRelatorio, filtroRelatorio.ExtensaoRelatorio, filtroRelatorio.UsuarioQueSolicitou));
            if (relatorio == null)
                return 0;

            var relatorioFiltrado = relatorio?.FirstOrDefault(r => r.FiltrosUsados == filtroRelatorio.FiltrosUsados);
            if (relatorioFiltrado == null)
                return 0;

            return relatorioFiltrado.Id;
        }
    }
}
