using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoAbandono
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAbandono> indicadores);
        Task<IEnumerable<PainelEducacionalConsolidacaoAbandono>> ObterConsolidacaoAsync(int anoLetivo, string codigoDre, string codigoUe);
    }
}
