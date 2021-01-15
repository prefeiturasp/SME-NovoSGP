using MediatR;
using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IServicoEol servicoEOL;

        public ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQueryHandler(IServicoEol servicoEOL, IMediator mediator):base(mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Handle(ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery request, CancellationToken cancellationToken)
        {
            var turmasEOL = await servicoEOL.ObterTurmasParaCopiaPlanoAnual(request.CodigoRF, request.ComponenteCurricularId, request.TurmaId);
            if (turmasEOL != null && turmasEOL.Any())
            {
                var idsTurmas = turmasEOL.Select(c => c.TurmaId.ToString());
                turmasEOL = await mediator.Send(new ValidaSeTurmasPossuemPlanoAnualQuery(idsTurmas.ToArray(), request.ConsideraHistorico)); ;
            }
            return turmasEOL;
        }
    }
}
