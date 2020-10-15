using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterObjetivosPorDisciplinaUseCase
    {
        Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(DateTime dataReferencia, long turmaId, long componenteCurricularId, long disciplinaId, bool regencia = false);
    }
}