using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public interface IObterSinteseConselhoDeClasseUseCase 
    {
        Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> Executar(ConselhoClasseSinteseDto conselhoClasseSinteseDto);
    }
}