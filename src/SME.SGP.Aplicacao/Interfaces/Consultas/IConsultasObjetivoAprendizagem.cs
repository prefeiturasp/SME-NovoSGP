using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasObjetivoAprendizagem
    {
        Task<bool> DisciplinaPossuiObjetivosDeAprendizagem(long codigoDisciplina);

        Task<IEnumerable<ObjetivoAprendizagemDto>> Filtrar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto);

        Task<IEnumerable<ObjetivoAprendizagemDto>> Listar();

        Task<IEnumerable<ComponenteCurricularSimplificadoDto>> ObterDisciplinasDoBimestrePlanoAnual(int ano, int bimestre, long turmaId, long componenteCurricularId);

        Task<IEnumerable<ObjetivoAprendizagemDto>> ObterObjetivosPlanoDisciplina(int ano, int bimestre, long turmaId, long componenteCurricularId, long disciplinaId);
    }
}