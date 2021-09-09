using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesAreasConhecimentoQueryHandler : IRequestHandler<ObterComponentesAreasConhecimentoQuery, IEnumerable<DisciplinaDto>>
    {
        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.ComponentesCurricularesTurma
                .Where(c => (!c.Regencia && request.AreasConhecimento.Select(a => a.CodigoComponenteCurricular).Contains(c.CodigoComponenteCurricular)) ||
                             (c.Regencia && request.AreasConhecimento.Select(a => a.CodigoComponenteCurricular).Any(cr =>
                              c.CodigoComponenteCurricular == cr)))
                .OrderBy(cc => cc.Nome));
        }
    }
}
