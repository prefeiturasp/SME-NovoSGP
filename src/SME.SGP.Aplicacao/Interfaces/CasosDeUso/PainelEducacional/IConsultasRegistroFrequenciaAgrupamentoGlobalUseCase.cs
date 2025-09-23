using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasRegistroFrequenciaAgrupamentoGlobalUseCase
    {
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>> ObterFrequencia(int anoLetivo, string codigoDre, string codigoUe);
    }
}
