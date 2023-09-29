using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
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
                .Select(a => a.CodigoComponenteCurricular).ToList();

            TratarComponentesRegencia(request, listaCodigosComponentes);

            if (listaCodigosComponentes.EhNulo() || !listaCodigosComponentes.Any())
                return default;

            return await mediator
                .Send(new ObterAreasConhecimentoComponenteCurricularQuery(listaCodigosComponentes.Distinct().ToArray()));
        }

        private async Task TratarComponentesRegencia(ObterAreasConhecimentoQuery request, List<long> listaCodigosComponentes)
        {
            if (request.ConsideraRegencia)
            {
                var codigosTurmas = request.ComponentesCurriculares.Where(c => !string.IsNullOrEmpty(c.TurmaCodigo))
                    .Select(cc => cc.TurmaCodigo)
                    .Where(codigoTurma => codigoTurma.NaoEhNulo())
                    .Distinct();
                foreach (var ct in codigosTurmas)
                {
                    var componentesRegencia = await mediator
                        .Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(ct, situacaoConselho: true));

                    if (componentesRegencia.NaoEhNulo() && componentesRegencia.Any())
                        listaCodigosComponentes.AddRange(componentesRegencia.Select(cr => cr.CodigoComponenteCurricular));
                }
                
            }
        }
    }
}
