using MediatR;
using SME.SGP.Aplicacao.Queries;
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
                .Where(cc => request.ConsideraRegencia || (!request.ConsideraRegencia && !cc.Regencia))
                .Select(a => a.CodigoComponenteCurricular);

            if (request.ConsideraRegencia)
            {
                var codigosTurmas = request.ComponentesCurriculares.Where(c => !string.IsNullOrEmpty(c.TurmaCodigo))
                    .Select(cc => cc.TurmaCodigo)
                    .Where(codigoTurma => codigoTurma != null)
                    .Distinct();

                codigosTurmas.ToList().ForEach(ct =>
                {
                    var componentesRegencia = mediator
                        .Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(ct))
                        .Result;

                    listaCodigosComponentes = listaCodigosComponentes
                        .Concat(componentesRegencia.Select(cr => cr.CodigoComponenteCurricular));
                });
            }

            return await mediator
                .Send(new ObterAreasConhecimentoComponenteCurricularQuery(listaCodigosComponentes.Distinct().ToArray()));
        }
    }
}
