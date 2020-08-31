using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface ISalvarObservacaoCartaIntencoesUseCase
    {
        Task<AuditoriaDto> Executar(long cartaIntencoesId, SalvarObservacaoCartaIntencoesDto salvarObservacaoCartaIntencoesDto);
    }
}