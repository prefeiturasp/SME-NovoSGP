using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoSondagemEscritaUe
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoSondagemEscritaUe> indicadores);
        Task<IEnumerable<PainelEducacionalConsolidacaoSondagemEscritaUe>> ObterConsolidacaoAsync(int anoLetivo, int bimestre, string codigoUe, string codigoDre);
    }
}
