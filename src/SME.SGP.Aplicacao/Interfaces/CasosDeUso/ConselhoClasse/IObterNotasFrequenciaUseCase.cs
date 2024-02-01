using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public interface IObterNotasFrequenciaUseCase 
    {
        Task<ConselhoClasseAlunoNotasConceitosRetornoDto> Executar(ConselhoClasseNotasFrequenciaDto notasFrequenciaDto);
    }
}