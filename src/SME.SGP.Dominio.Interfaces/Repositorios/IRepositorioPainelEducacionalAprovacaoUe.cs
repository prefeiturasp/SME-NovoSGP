
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SME.SGP.Infra.Dtos.PainelEducacional.PainelEducacionalAprovacaoUeDto;


namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAprovacaoUe
    {
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores);
        Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeItemDto>> ObterAprovacao(FiltroAprovacaoUeDto filtro);
    }
}
