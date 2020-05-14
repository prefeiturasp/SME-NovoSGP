using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasObjetivoAprendizagem
    {
        bool DisciplinaPossuiObjetivosDeAprendizagem(long codigoDisciplina);

        Task<IEnumerable<ObjetivoAprendizagemDto>> Filtrar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto);

        Task<IEnumerable<ObjetivoAprendizagemDto>> Listar();

        Task<ObjetivoAprendizagemSimplificadoDto> ObterAprendizagemSimplificadaPorId(long id);

        Task<IEnumerable<ComponenteCurricularSimplificadoDto>> ObterDisciplinasDoBimestrePlanoAnual(int ano, int bimestre, long turmaId, long componenteCurricularId);

        Task<long> ObterIdPorObjetivoAprendizagemJurema(long planoId, long objetivoAprendizagemJuremaId);

        Task<IEnumerable<ObjetivoAprendizagemDto>> ObterObjetivosPlanoDisciplina(int ano, int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool regencia = false);
    }
}