using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAprovacao
    {
        Task<IEnumerable<DadosParaConsolidarAprovacao>> ObterIndicadores(long[] turmasIds);
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacao> indicadores);
        Task<IEnumerable<PainelEducacionalConsolidacaoAprovacao>> ObterAprovacao(int anoLetivo, string codigoDre);
    }
}
