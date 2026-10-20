using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralIndexadaPorAlunosQueryHandler : IRequestHandler<ObterFrequenciaGeralIndexadaPorAlunosQuery, Dictionary<string, FrequenciaAluno>>
    {
        private readonly IMediator mediator;

        public ObterFrequenciaGeralIndexadaPorAlunosQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Dictionary<string, FrequenciaAluno>> Handle(ObterFrequenciaGeralIndexadaPorAlunosQuery request, CancellationToken cancellationToken)
        {
            var codigos = request.CodigosAlunos?
                .Where(codigo => !string.IsNullOrWhiteSpace(codigo))
                .Distinct()
                .ToArray() ?? Array.Empty<string>();

            if (!codigos.Any())
                return new Dictionary<string, FrequenciaAluno>();

            var frequenciasAlunos = await mediator.Send(new ObterFrequenciaGeralPorAlunosTurmaEComponenteQuery(codigos, request.TurmaCodigo, request.ComponenteCurricularCodigo), cancellationToken);

            return frequenciasAlunos.ToDicionarioPorAluno();
        }
    }
}
