using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsultaFrequenciaGeralAlunoQueryHandler : IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>
    {
        private readonly IMediator mediator;
        public ObterConsultaFrequenciaGeralAlunoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(ObterConsultaFrequenciaGeralAlunoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.TurmaCodigo), cancellationToken);
            var turmasItinerarioEnsinoMedio = await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance, cancellationToken);

            string[] turmasCodigos;

            if (turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
            {
                var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();
                
                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
                tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));
                
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, request.AlunoCodigo, tiposParaConsulta, semestre: turma.Semestre != 0 ? turma.Semestre : null), cancellationToken);
            }
            else
                turmasCodigos = new string[1] { turma.CodigoTurma };

            if (!turmasCodigos.Any())
                turmasCodigos = new[] { turma.CodigoTurma };

            return await mediator.Send(new ObterConsultaFrequenciaGeralAlunoPorTurmasQuery(request.AlunoCodigo, turmasCodigos, request.ComponenteCurricularCodigo, turma), cancellationToken);
        }
    }
}
