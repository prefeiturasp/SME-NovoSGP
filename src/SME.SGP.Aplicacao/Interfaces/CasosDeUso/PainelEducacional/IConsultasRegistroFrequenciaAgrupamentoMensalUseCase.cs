using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasRegistroFrequenciaAgrupamentoMensalUseCase
    {
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>> ObterFrequencia(string codigoDre, string codigoUe);
    }
}
