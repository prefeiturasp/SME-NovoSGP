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

            await TratarComponentesRegencia(request, listaCodigosComponentes);
            await TratarComponentesTerritorio(request, listaCodigosComponentes);

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

        private async Task TratarComponentesTerritorio(ObterAreasConhecimentoQuery request, List<long> listaCodigosComponentes)
        {
            if (request.ComponentesCurriculares.Any(cc => cc.TerritorioSaber))
            {
                var componentesTerritorio = request.ComponentesCurriculares
                    .Where(cc => cc.TerritorioSaber).ToList();

                foreach (var cc in componentesTerritorio)
                {
                    if (cc.CodigoComponenteCurricular > 0 && !string.IsNullOrEmpty(cc.TurmaCodigo))
                    {
                        var componenteTerritorioEquivalente = await mediator
                        .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(cc.CodigoComponenteCurricular, cc.TurmaCodigo, null));

                        if (componenteTerritorioEquivalente.NaoEhNulo() && componenteTerritorioEquivalente.Any())
                        {
                            var codigoConsiderado = componenteTerritorioEquivalente.Select(ct => ct.codigoComponente)
                                .Except(new string[] { cc.CodigoComponenteCurricular.ToString() }).FirstOrDefault();
                            if (codigoConsiderado.NaoEhNulo() && long.Parse(codigoConsiderado) < cc.CodigoComponenteCurricular)
                                listaCodigosComponentes.Add(long.Parse(codigoConsiderado));
                        }
                    }
                }
            }
        }
    }
}
