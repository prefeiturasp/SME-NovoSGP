using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoTaxaAlfabetizacao> indicadores);
        Task<IEnumerable<PainelEducacionalConsolidacaoTaxaAlfabetizacao>> ObterConsolidacaoAsync(int anoLetivo, string codigoDre, string codigoUe);
    }
}
