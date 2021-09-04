using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.CasosDeUso.AreaDoConhecimento
{
    public class ObterComponentesDasAreasDeConhecimentoUseCase : IObterComponentesDasAreasDeConhecimentoUseCase
    {
        public IEnumerable<DisciplinaDto> ObterComponentesDasAreasDeConhecimento(IEnumerable<DisciplinaDto> componentesCurricularesDaTurma, IEnumerable<AreaDoConhecimentoDto> areaDoConhecimento)
        {
            return componentesCurricularesDaTurma.Where(c => (!c.Regencia && areaDoConhecimento.Select(a => a.CodigoComponenteCurricular).Contains(c.CodigoComponenteCurricular)) ||
                                                             (c.Regencia && areaDoConhecimento.Select(a => a.CodigoComponenteCurricular).Any(cr =>
                                                              c.CodigoComponenteCurricular == cr))).OrderBy(cc => cc.Nome);
        }
    }
}
