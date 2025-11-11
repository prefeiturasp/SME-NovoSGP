using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioEducacaoIntegralConsulta
    {
        Task<IEnumerable<DadosParaConsolidarEducacaoIntegralDto>> ObterEducacaoIntegral(FiltroPainelEducacionalEducacaoIntegral filtro);
    }
}
