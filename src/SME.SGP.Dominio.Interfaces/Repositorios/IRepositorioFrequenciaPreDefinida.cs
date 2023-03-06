using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaPreDefinida
    {
        Task<IEnumerable<FrequenciaPreDefinidaDto>> Listar(long turmaId, long componenteCurricularId, string alunoCodigo);
        Task RemoverPorCCIdETurmaId(long componenteCurricularId, long turmaId, string[] alunosComFrequenciaRegistrada);
        Task Salvar(FrequenciaPreDefinida frequenciaPreDefinida);
        Task Atualizar(FrequenciaPreDefinida frequenciaPreDefinida);
        Task<FrequenciaPreDefinidaDto> ObterPorTurmaECCEAlunoCodigo(long turmaId, long componenteCurricularId, string alunoCodigo);
        Task<IEnumerable<FrequenciaPreDefinidaDto>> ObterPorTurmaEComponente(long turmaId, long componenteCurricularId);
        Task<IEnumerable<FrequenciaPreDefinida>> ObterListaFrequenciaPreDefinida(long turmaId, long componenteCurricularId);
        Task<bool> InserirVarios(IEnumerable<FrequenciaPreDefinida> registros);
    }
}