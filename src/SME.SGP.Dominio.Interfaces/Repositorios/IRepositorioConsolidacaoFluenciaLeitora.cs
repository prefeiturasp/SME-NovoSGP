using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioConsolidacaoFluenciaLeitora
    {
        Task ExcluirConsolidacaoFluenciaLeitora();
        Task<IEnumerable<ConsolidacaoPainelEducacionalFluenciaLeitora>> ObterFluenciaLeitora(string codigoDre);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> registros);
    }
}
