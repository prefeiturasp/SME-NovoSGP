
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAprovacaoUe
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores);
        Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>> ObterAprovacao(
            int anoLetivo,
            string codigoUe,
            int modalidadeId,
            Paginacao paginacao);
    }
}
