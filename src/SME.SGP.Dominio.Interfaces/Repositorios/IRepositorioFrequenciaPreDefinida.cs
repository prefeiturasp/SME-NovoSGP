using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaPreDefinida
    {
        Task<IEnumerable<FrequenciaPreDefinidaDto>> Listar(long turmaId, long componenteCurricularId, string alunoCodigo);
    }
}