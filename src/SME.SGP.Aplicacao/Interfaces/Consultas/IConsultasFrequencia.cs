using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFrequencia
    {
        Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId);
    }
}