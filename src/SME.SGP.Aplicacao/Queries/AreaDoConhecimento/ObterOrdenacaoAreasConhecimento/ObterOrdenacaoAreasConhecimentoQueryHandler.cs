using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOrdenacaoAreasConhecimentoQueryHandler : IRequestHandler<ObterOrdenacaoAreasConhecimentoQuery, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>
    {
        private readonly IMediator mediator;

        public ObterOrdenacaoAreasConhecimentoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>> Handle(ObterOrdenacaoAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            var listaGrupoMatrizId = request.ComponentesCurricularesTurma?
                .Select(a => a.GrupoMatrizId)?
                .Distinct()
                .ToArray();

            var listaAreaConhecimentoId = request.AreasConhecimento?
                .Select(a => a.Id)
                .ToArray();

            return await mediator
                .Send(new ObterComponenteCurricularGrupoAreaOrdenacaoQuery(listaGrupoMatrizId, listaAreaConhecimentoId));
        }
    }
}
