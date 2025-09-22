using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasIdebPainelEducacionalUseCase
    {
        Task<PainelEducacionalIdebAgrupamentoDto> ObterIdeb(FiltroPainelEducacionalIdeb filtro);
    }
}
