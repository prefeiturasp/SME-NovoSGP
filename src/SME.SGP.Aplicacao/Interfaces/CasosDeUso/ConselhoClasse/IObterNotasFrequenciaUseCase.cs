using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{

    public interface IObterNotasFrequenciaUseCase
    {
        Task<ConselhoClasseAlunoNotasConceitosRetornoDto> Executar(ConselhoClasseNotasFrequenciaDto notasFrequenciaDto);
    }
}