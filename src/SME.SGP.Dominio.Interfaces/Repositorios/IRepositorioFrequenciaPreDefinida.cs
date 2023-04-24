using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaPreDefinida
    {
        Task RemoverPorCCIdETurmaId(long componenteCurricularId, long turmaId, string[] alunosComFrequenciaRegistrada);
        Task Salvar(FrequenciaPreDefinida frequenciaPreDefinida);
        Task Atualizar(FrequenciaPreDefinida frequenciaPreDefinida);
        Task<bool> InserirVarios(IEnumerable<FrequenciaPreDefinida> registros);
    }
}