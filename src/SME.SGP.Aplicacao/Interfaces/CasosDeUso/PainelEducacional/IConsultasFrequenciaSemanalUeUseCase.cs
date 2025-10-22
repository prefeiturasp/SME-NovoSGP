using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasFrequenciaSemanalUeUseCase
    {
        Task<IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>> ObterFrequenciaSemanalUe(string codigoUe, int anoLetivo);
    }
}
