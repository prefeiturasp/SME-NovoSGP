using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasFluenciaLeitoraUeUseCase
    {
        Task<IEnumerable<PainelEducacionalFluenciaLeitoraUeDto>> ObterFluenciaLeitoraUe(FiltroPainelEducacionalFluenciaLeitoraUe filtro);
    }
}
