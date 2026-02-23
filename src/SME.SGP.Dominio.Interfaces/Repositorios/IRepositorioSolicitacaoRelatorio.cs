using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioSolicitacaoRelatorio : IRepositorioBase<SolicitacaoRelatorio>
    {
        Task<IEnumerable<SolicitacaoRelatorio>> BuscarPorFiltrosExatosAsync(FiltroRelatorioBase filtros,TipoRelatorio? tipoRelatorio = null,StatusSolicitacao? statusSolicitacao = null);
        Task<bool> RelatorioJaSolicitadoAsync(FiltroRelatorioBase filtros,TipoRelatorio tipoRelatorio,string usuarioQueSolicitou);
    }
}
