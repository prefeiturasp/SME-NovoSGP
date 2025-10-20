using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoNota 
    {
        Task LimparConsolidacaoAsync(int anoLetivo);
        Task SalvarConsolidacaoAsync(IList<PainelEducacionalConsolidacaoNota> indicadores);
        Task SalvarConsolidacaoUeAsync(IList<PainelEducacionalConsolidacaoNotaUe> indicadores);
    }
}
