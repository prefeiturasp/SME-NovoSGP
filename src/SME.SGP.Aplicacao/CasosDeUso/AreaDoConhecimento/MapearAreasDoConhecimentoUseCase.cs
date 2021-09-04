using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.CasosDeUso.AreaDoConhecimento
{
    public class MapearAreasDoConhecimentoUseCase : IMapearAreasDoConhecimentoUseCase
    {
        public IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>> MapearAreasDoConhecimento(IEnumerable<DisciplinaDto> componentesCurricularesDaTurma, IEnumerable<AreaDoConhecimentoDto> areasDoConhecimentos, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto> grupoAreaOrdenacao, long grupoMatrizId)
        {
            return areasDoConhecimentos.Where(a => componentesCurricularesDaTurma.Where(cc => !cc.Regencia).Select(cc => cc.CodigoComponenteCurricular).Contains(a.CodigoComponenteCurricular) ||
                                                   (componentesCurricularesDaTurma.Any(cc => cc.Regencia) && componentesCurricularesDaTurma.Where(cc => cc.Regencia)
                                       .Select(r => r.CodigoComponenteCurricular).Contains(a.CodigoComponenteCurricular)))
                                       .Select(a => { a.DefinirOrdem(grupoAreaOrdenacao, grupoMatrizId); return a; }).GroupBy(g => (g.Nome, g.Ordem, g.Id))
                                       .OrderByDescending(c => c.Key.Id > 0 && !string.IsNullOrEmpty(c.Key.Nome))
                                       .ThenByDescending(c => c.Key.Ordem.HasValue).ThenBy(c => c.Key.Ordem)
                                       .ThenBy(c => c.Key.Nome, StringComparer.OrdinalIgnoreCase);
        }
    }
}
