using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento
{
    public interface IObterComponentesDasAreasDeConhecimentoUseCase
    {
        IEnumerable<DisciplinaDto> ObterComponentesDasAreasDeConhecimento(IEnumerable<DisciplinaDto> componentesCurricularesDaTurma, IEnumerable<AreaDoConhecimentoDto> areaDoConhecimento);
    }
}
