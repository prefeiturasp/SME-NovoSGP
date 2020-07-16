using SME.SGP.Infra;
using System;
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

        Task<IEnumerable<ComponenteCurricularSimplificadoDto>> ObterDisciplinasDoBimestrePlanoAnual(DateTime dataReferencia, long turmaId, long componenteCurricularId);

        long ObterIdPorObjetivoAprendizagemJurema(long planoId, long objetivoAprendizagemJuremaId);

        Task<IEnumerable<ObjetivoAprendizagemDto>> ObterObjetivosPlanoDisciplina(DateTime dataReferencia, long turmaId, long componenteCurricularId, long disciplinaId, bool regencia = false);
        Task<bool> ComponentePossuiObjetivosOpcionais(long componenteCurricularCodigo, bool regencia, bool turmaEspecial);
    }
}