using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoEducacaoIntegral
    {
        Task<int?> ObterUltimoAnoConsolidado();
        Task LimparConsolidacao(int ano);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoEducacaoIntegral> indicadores);
    }
}
