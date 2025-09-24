using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasRegistroFrequenciaAgrupamentoRankingUseCase
    {
        Task<PainelEducacionalRegistroFrequenciaRankingDto> ObterFrequencia(int anoLetivo, string codigoDre, string codigoUe);
    }
}
