using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{

    public interface IObterSinteseConselhoDeClasseUseCase
    {
        Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> Executar(ConselhoClasseSinteseDto conselhoClasseSinteseDto);
    }
}