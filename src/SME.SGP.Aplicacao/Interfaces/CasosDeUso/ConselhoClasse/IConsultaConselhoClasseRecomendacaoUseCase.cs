using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public interface IConsultaConselhoClasseRecomendacaoUseCase 
    {
        Task<ConsultasConselhoClasseRecomendacaoConsultaDto> Executar(ObterConselhoClasseRecomendacaoConsultaDto recomendacaoDto);
    }
}