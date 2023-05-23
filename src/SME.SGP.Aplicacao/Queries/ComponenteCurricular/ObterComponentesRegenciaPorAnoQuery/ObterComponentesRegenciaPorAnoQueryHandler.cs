using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesRegenciaPorAnoQueryHandler : IRequestHandler<ObterComponentesRegenciaPorAnoQuery, IEnumerable<DisciplinaResposta>>
    {
        private readonly IMediator mediator;

        public ObterComponentesRegenciaPorAnoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterComponentesRegenciaPorAnoQuery request,
            CancellationToken cancellationToken)
        {
            var componentesCurriculares = await mediator.Send(new ObterComponentesRegenciaPorAnoEolQuery(request.AnoTurma));

            return MapearComponentes(componentesCurriculares.OrderBy(c => c.Descricao));
        }
        
        private IEnumerable<DisciplinaResposta> MapearComponentes(IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            foreach (var componenteCurricular in componentesCurriculares)
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricularPai = componenteCurricular.CodigoComponenteCurricularPai,
                    CodigoComponenteCurricular = componenteCurricular.Codigo,
                    Nome = componenteCurricular.Descricao,
                    Regencia = componenteCurricular.Regencia,
                    TerritorioSaber = componenteCurricular.TerritorioSaber,
                    Compartilhada = componenteCurricular.Compartilhada,
                    LancaNota = componenteCurricular.LancaNota,

                };
        }
    }
}
