using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasIdepPainelEducacionalUseCase
    {
        Task<PainelEducacionalIdepAgrupamentoDto> ObterIdepPorAnoEtapa(int anoLetivo, string etapa, string codigoDre);
    }
}
