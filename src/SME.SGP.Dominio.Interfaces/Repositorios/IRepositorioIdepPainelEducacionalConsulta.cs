using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioIdepPainelEducacionalConsulta : IRepositorioBase<PainelEducacionalIdep>
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterTodosIdep();
        Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterIdepPorAnoEtapa(int ano, int etapa);
    }
}
