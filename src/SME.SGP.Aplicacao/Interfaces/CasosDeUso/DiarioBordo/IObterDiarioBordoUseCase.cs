using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterDiarioBordoUseCase
    {
        Task<DiarioBordoDto> Executar(long aulaId, long componenteCurricularId);
    }
}
