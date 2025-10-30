using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoDistorcaoIdade
    {
        Task<int?> ObterUltimoAnoConsolidadoAsync();
        Task LimparConsolidacao(int ano);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoDistorcaoIdade> indicadores);
    }
}
