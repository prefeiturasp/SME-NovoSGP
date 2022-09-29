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
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.TurmaCodigo));
            var turmasitinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

            string[] turmasCodigos;

            if (turma.DeveVerificarRegraRegulares() || turmasitinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
            {
                var turmasCodigosParaConsulta = new List<int>() { (int)turma.TipoTurma };
                turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                turmasCodigosParaConsulta.AddRange(turmasitinerarioEnsinoMedio.Select(s => s.Id));
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, request.AlunoCodigo, turmasCodigosParaConsulta, turma.Historica));
            }
            else
                turmasCodigos = new string[1] { turma.CodigoTurma };

            if (!turmasCodigos.Any())
                turmasCodigos = new string[] { turma.CodigoTurma };

            return await mediator.Send(new ObterConsultaFrequenciaGeralAlunoPorTurmasQuery(request.AlunoCodigo, turmasCodigos, request.ComponenteCurricularCodigo, turma));
        }
    }
}
