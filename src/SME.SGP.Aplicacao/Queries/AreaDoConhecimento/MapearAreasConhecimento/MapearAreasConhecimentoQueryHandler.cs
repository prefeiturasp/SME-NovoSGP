using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.MapearAreasConhecimento
{
    public class MapearAreasConhecimentoQueryHandler : IRequestHandler<MapearAreasConhecimentoQuery, IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>>
    {
        public async Task<IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>> Handle(MapearAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.AreasConhecimentos.Where(a => request.ComponentesCurricularesTurma.Where(cc => !cc.Regencia).Select(cc => cc.CodigoComponenteCurricular).Contains(a.CodigoComponenteCurricular) ||
                                                                              (request.ComponentesCurricularesTurma.Any(cc => cc.Regencia) && request.ComponentesCurricularesTurma.Where(cc => cc.Regencia)
                                                                   .Select(r => r.CodigoComponenteCurricular).Contains(a.CodigoComponenteCurricular)))
                                                                   .Select(a => { a.DefinirOrdem(request.GruposAreaOrdenacao, request.GrupoMatrizId); return a; }).GroupBy(g => (g.Nome, g.Ordem, g.Id))
                                                                   .OrderByDescending(c => c.Key.Id > 0 && !string.IsNullOrEmpty(c.Key.Nome))
                                                                   .ThenByDescending(c => c.Key.Ordem.HasValue).ThenBy(c => c.Key.Ordem)
                                                                   .ThenBy(c => c.Key.Nome, StringComparer.OrdinalIgnoreCase));
        }
    }
}
