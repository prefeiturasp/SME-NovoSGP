using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoAprendizagem
    {
        Task AtualizarAsync(ObjetivoAprendizagem objetivoAprendizagem);

        Task<IEnumerable<ObjetivoAprendizagem>> ListarAsync();

        Task<ObjetivoAprendizagem> ObterPorIdAsync(long id);

        Task ReativarAsync(long id);

        Task<IEnumerable<ObjetivoAprendizagemDto>> ObterPorAnoEComponenteCurricularId(AnoTurma ano, long componenteCurricularId);

        Task<IEnumerable<ObjetivoAprendizagemDto>> ObterPorComponenteCurricularJuremaIds(long[] juremaIds);
        
        Task InserirAsync(ObjetivoAprendizagem objetivoAprendizagem);
        
        Task<bool> ExistePorCodigoCompletoEAnoTurmaAsync(string codigoCompleto, string anoTurma);
    }
}