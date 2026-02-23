using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasPlanosAEEPainelEducacionalUseCase
    {
        Task<IEnumerable<PainelEducacionalPlanoAEEDto>> ObterPlanosAEE(FiltroPainelEducacionalPlanosAEE filtro);
    }
}
