using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais
    {
        Task<IEnumerable<DadosParaConsolidarInformacoesEducacionaisDto>> ObterDadosParaConsolidarInformacoesEducacionais(int anoLetivo, string[] codigosUes);
        Task LimparConsolidacao();
        Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoInformacoesEducacionais> indicadores);
    }
}
