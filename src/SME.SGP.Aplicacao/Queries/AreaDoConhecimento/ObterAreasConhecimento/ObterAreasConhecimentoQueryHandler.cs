using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAreasConhecimentoQueryHandler : IRequestHandler<ObterAreasConhecimentoQuery, IEnumerable<AreaDoConhecimentoDto>>
    {
        private readonly IMediator mediator;

        public ObterAreasConhecimentoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AreaDoConhecimentoDto>> Handle(ObterAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            var listaCodigosComponentes = request.ComponentesCurriculares
                .Where(cc => !cc.Regencia)
                .Select(a => a.CodigoComponenteCurricular);

            return await mediator
                .Send(new ObterAreasConhecimentoComponenteCurricularQuery(listaCodigosComponentes.Distinct().ToArray()));
        }
    }
}
