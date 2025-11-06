using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoFluenciaLeitoraUe
    {
        Task<IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>> ObterDadosParaConsolidarFluenciaLeitoraUe(int anoLetivo);
        Task LimparConsolidacao(int ano);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoFluenciaLeitoraUe> indicadores);
    }
}
