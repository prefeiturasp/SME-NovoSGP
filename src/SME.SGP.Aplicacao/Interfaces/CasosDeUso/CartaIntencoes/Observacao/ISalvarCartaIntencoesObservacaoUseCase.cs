using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface ISalvarCartaIntencoesObservacaoUseCase
    {
        Task<AuditoriaDto> Executar(string turmaCodigo,long componenteCurricularId, SalvarCartaIntencoesObservacaoDto salvarCartaIntencoesObservacaoDto);
    }
}