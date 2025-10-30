using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoProficienciaIdeb
    {        
        Task LimparConsolidacaoPorAnoAsync(int anoLetivo);
        Task SalvarConsolidacaoAsync(IList<PainelEducacionalConsolidacaoProficienciaIdebUe> indicadores);
    }
}