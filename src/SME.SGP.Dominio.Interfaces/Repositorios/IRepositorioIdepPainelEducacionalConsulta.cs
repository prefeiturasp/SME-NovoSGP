using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioIdepPainelEducacionalConsulta : IRepositorioBase<PainelEducacionalIdep>
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterTodosIdep();
        Task<IEnumerable<PainelEducacionalIdepDto>> ObterIdepPorAnoEtapa(int anoLetivo, string etapa, string codigoDre);
    }
}
