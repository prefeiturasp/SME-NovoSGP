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
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(new[] { turmaCodigo }), cancellationToken);

            var turmasCodigos = turmas.Select(c => c.CodigoTurma).ToArray();
            
            return (await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery(turmasCodigos, request.Bimestre), cancellationToken))
                .Where(c => c.AlunoCodigo == request.AlunoCodigo && c.ConselhoClasseId == request.ConselhoClasseId);
        }
    }
}