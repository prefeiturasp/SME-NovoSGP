using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasInformacoesPapUseCase
    {
        Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterInformacoesPap(int anoLetivo, string codigoDre, string codigoUe);
    }
}
