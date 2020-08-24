using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarAnotacaoFrequenciaAlunoUseCase
    {
        Task<AuditoriaDto> Executar(SalvarAnotacaoFrequenciaAlunoDto dto);
    }
}