using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioFrequenciaSemanalUe
    {
        Task<IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>> ObterFrequenciaSemanalUe(string codigoDre, int anoLetivo);       
    }
}
