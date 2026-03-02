using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasEducacaoIntegralPainelEducacionalUseCase
    {
        Task<IEnumerable<PainelEducacionalEducacaoIntegralDto>> ObterEducacaoIntegral(FiltroPainelEducacionalEducacaoIntegral filtro);
    }
}
