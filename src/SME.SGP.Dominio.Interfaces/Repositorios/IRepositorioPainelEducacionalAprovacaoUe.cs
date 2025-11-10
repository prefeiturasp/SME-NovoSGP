using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAprovacaoUe
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores);
        Task<IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe>> ObterAprovacao(int anoLetivo, string codigoUe);
    }
}
