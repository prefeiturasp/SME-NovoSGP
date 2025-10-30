using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoNotaConsulta
    {
        Task<int?> ObterUltimoAnoConsolidadoAsync();
        Task<IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos>> ObterDadosBrutosPorAnoLetivoAsync(int anoLetivo);
    }
}
