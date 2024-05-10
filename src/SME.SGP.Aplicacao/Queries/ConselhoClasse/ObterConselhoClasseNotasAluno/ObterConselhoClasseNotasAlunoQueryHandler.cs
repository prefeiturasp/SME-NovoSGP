using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseNotasAlunoQueryHandler : IRequestHandler<ObterConselhoClasseNotasAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IMediator mediator;

        public ObterConselhoClasseNotasAlunoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterConselhoClasseNotasAlunoQuery request, CancellationToken cancellationToken)
        {
            var turmaCodigo = await mediator.Send(new ObterTurmaPorConselhoClasseIdQuery(request.ConselhoClasseId), cancellationToken);

            return (await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery(new[] { turmaCodigo }, request.Bimestre, alunoCodigo: request.AlunoCodigo, tipoCalendario: request.TipoCalendario), cancellationToken))
                .Where(c => c.AlunoCodigo == request.AlunoCodigo && c.ConselhoClasseId == request.ConselhoClasseId && (!request.ComponenteCurricularId.HasValue || (request.ComponenteCurricularId.HasValue && c.ComponenteCurricularCodigo.Equals(request.ComponenteCurricularId.Value))));
        }
    }
}