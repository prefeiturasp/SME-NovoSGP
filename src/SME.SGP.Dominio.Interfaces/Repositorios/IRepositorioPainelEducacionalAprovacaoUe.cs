using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAprovacaoUe
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores);
        Task<(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> Itens, int TotalRegistros)> ObterAprovacao(
     int anoLetivo, string codigoUe, int modalidadeId, int numeroPagina, int numeroRegistros);
    }
}
