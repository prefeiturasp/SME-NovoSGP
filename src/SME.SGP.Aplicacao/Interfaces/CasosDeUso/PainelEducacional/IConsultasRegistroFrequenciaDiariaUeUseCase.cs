using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasRegistroFrequenciaDiariaUeUseCase
    {
        Task<FrequenciaDiariaUeDto> ObterFrequenciaDiariaPorUe(FiltroFrequenciaDiariaUeDto filtro);
    }
}
