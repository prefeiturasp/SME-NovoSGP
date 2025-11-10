using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAprovacaoUe
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores);
        Task<PaginacaoResultadoDto<PainelEducacionalConsolidacaoAprovacaoUe>> ObterAprovacao(int anoLetivo, string codigoUe, int numeroPagina, int numeroRegistros);
    }
}
