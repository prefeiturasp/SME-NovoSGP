using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
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
        private readonly IMapearAreasDoConhecimentoUseCase mapearAreasDoConhecimentoUseCase;

        public MapearAreasConhecimentoQueryHandler(IMapearAreasDoConhecimentoUseCase mapearAreasDoConhecimentoUseCase)
        {
            this.mapearAreasDoConhecimentoUseCase = mapearAreasDoConhecimentoUseCase ?? throw new ArgumentNullException(nameof(mapearAreasDoConhecimentoUseCase));
        }

        public async Task<IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>> Handle(MapearAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(mapearAreasDoConhecimentoUseCase
                .MapearAreasDoConhecimento(request.ComponentesCurricularesTurma,
                                           request.AreasConhecimentos,
                                           request.GruposAreaOrdenacao,
                                           request.GrupoMatrizId));
        }
    }
}
