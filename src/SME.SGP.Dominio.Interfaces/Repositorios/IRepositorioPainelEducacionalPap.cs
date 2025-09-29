using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalPap
    {
        Task<int?> ObterUltimoAnoConsolidado();
        Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterConsolidacoesSmePorAno(int ano);
        Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterConsolidacoesDrePorAno(int ano, string codigoDre);
        Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterConsolidacoesUePorAno(int ano, string codigoDre, string codigoUe);
        Task ExcluirConsolidacaoApartirDoAno(int ano);
        Task SalvarConsolidacaoSme(IList<PainelEducacionalConsolidacaoPapSme> consolidacao);        
        Task SalvarConsolidacaoUe(IList<PainelEducacionalConsolidacaoPapUe> consolidacao);
        Task SalvarConsolidacaoDre(IList<PainelEducacionalConsolidacaoPapDre> consolidacao);
    }
}