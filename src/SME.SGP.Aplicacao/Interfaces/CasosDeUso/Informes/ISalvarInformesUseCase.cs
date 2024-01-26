using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarInformesUseCase
    {
        Task<AuditoriaDto> Executar(InformesDto informesDto);
    }
}
