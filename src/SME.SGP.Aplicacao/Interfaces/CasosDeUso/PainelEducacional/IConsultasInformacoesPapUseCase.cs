using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasInformacoesPapUseCase
    {
        Task<IndicadoresPapDto> ObterInformacoesPap(int anoLetivo, string codigoDre, string codigoUe);
    }
}
