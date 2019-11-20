using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasFrequencia
    {
        Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId);
    }
}