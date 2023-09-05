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
                .Select(a => a.CodigoComponenteCurricular).ToList();

            TratarComponentesRegencia(request, listaCodigosComponentes);
            TratarComponentesTerritorio(request, listaCodigosComponentes);

            if (listaCodigosComponentes == null || !listaCodigosComponentes.Any())
                return default;

            return await mediator
                .Send(new ObterAreasConhecimentoComponenteCurricularQuery(listaCodigosComponentes.Distinct().ToArray()));
        }

        private void TratarComponentesRegencia(ObterAreasConhecimentoQuery request, List<long> listaCodigosComponentes)
        {
            if (request.ConsideraRegencia)
            {
                var codigosTurmas = request.ComponentesCurriculares.Where(c => !string.IsNullOrEmpty(c.TurmaCodigo))
                    .Select(cc => cc.TurmaCodigo)
                    .Where(codigoTurma => codigoTurma != null)
                    .Distinct();

                codigosTurmas.ToList().ForEach(ct =>
                {
                    var componentesRegencia = mediator
                        .Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(ct, situacaoConselho: true))
                        .Result;

                    if (componentesRegencia != null && componentesRegencia.Any())
                        listaCodigosComponentes.AddRange(componentesRegencia.Select(cr => cr.CodigoComponenteCurricular));
                });
            }
        }

        private void TratarComponentesTerritorio(ObterAreasConhecimentoQuery request, List<long> listaCodigosComponentes)
        {
            /*if (request.ComponentesCurriculares.Any(cc => cc.TerritorioSaber))
            {
                var componentesTerritorio = request.ComponentesCurriculares
                    .Where(cc => cc.TerritorioSaber).ToList();

                componentesTerritorio.ForEach(cc =>
                {
                    if(cc.CodigoComponenteCurricular > 0 && !string.IsNullOrEmpty(cc.TurmaCodigo))
                    {
                        var componenteTerritorioEquivalente = mediator
                        .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(cc.CodigoComponenteCurricular, cc.TurmaCodigo, null)).Result;

                        if (componenteTerritorioEquivalente != null && componenteTerritorioEquivalente.Any())
                        {
                            var codigoConsiderado = componenteTerritorioEquivalente.Select(ct => ct.codigoComponente)
                                .Except(new string[] { cc.CodigoComponenteCurricular.ToString() }).FirstOrDefault();
                            if (codigoConsiderado != null && long.Parse(codigoConsiderado) < cc.CodigoComponenteCurricular)
                                listaCodigosComponentes.Add(long.Parse(codigoConsiderado));
                        }
                    }
                });
            }*/
        }
    }
}
