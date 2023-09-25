using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQueryHandler : AbstractUseCase, IRequestHandler<ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery, IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        public ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQueryHandler(IMediator mediator):base(mediator)
        {}

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Handle(ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery request, CancellationToken cancellationToken)
        {
            var turmasEOL = await mediator.Send(new ObterTurmasParaCopiaPlanoAnualQuery(request.CodigoRF, request.ComponenteCurricularId, request.TurmaId));
            if (turmasEOL.NaoEhNulo() && turmasEOL.Any())
            {
                var idsTurmas = turmasEOL.Select(c => c.TurmaId.ToString());
                turmasEOL = await mediator.Send(new ValidaSeTurmasPossuemPlanoAnualQuery(idsTurmas.ToArray(), request.ConsideraHistorico)); ;
            }
            return turmasEOL;
        }
    }
}
