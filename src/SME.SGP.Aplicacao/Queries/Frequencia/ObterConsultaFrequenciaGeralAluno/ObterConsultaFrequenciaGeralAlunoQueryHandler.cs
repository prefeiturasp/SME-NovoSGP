using MediatR;
using SME.SGP.Dominio.Enumerados;
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

            var turmasCodigos = new List<string>();
            var matriculaAlunoNaTurma = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(request.TurmaCodigo, request.AlunoCodigo, true));
            bool alunoEstaAtivoNaTurma = !matriculaAlunoNaTurma?.Inativo ?? true;

            if (turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
            {
                var tiposParaConsulta = new List<int>();

                if (turma.TipoTurma == TipoTurma.Regular && !alunoEstaAtivoNaTurma)
                    turmasCodigos.Add(turma.CodigoTurma);
                else
                    tiposParaConsulta.Add((int)turma.TipoTurma);

                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();
                
                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
                tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));
                
                var turmasAlunoAnoLetivo = (await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, request.AlunoCodigo, tiposParaConsulta, semestre: turma.Semestre != 0 ? turma.Semestre : null), cancellationToken))?.ToList();

                if (alunoEstaAtivoNaTurma)
                    await VerificaRegraRegularesAtivosEInativos(turmasAlunoAnoLetivo, request.AlunoCodigo);

                if (turmasAlunoAnoLetivo.Any())
                    turmasCodigos.AddRange(turmasAlunoAnoLetivo);
            }
            else
                turmasCodigos.Add(turma.CodigoTurma);

            if (!turmasCodigos.Any())
                turmasCodigos.Add(turma.CodigoTurma);

            return await mediator.Send(new ObterConsultaFrequenciaGeralAlunoPorTurmasQuery(request.AlunoCodigo, turmasCodigos.ToArray(), request.ComponenteCurricularCodigo, turma), cancellationToken);
        }

        public async Task VerificaRegraRegularesAtivosEInativos(List<string> turmasDoAlunoNoAno, string codigoAluno)
        {
            var dadosTurmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasDoAlunoNoAno.ToArray()));
            var turmasRegulares = dadosTurmas?.Where(d => d.TipoTurma == TipoTurma.Regular);

            if (turmasRegulares.Any())
            {
                foreach (var turma in turmasRegulares)
                {
                    var matriculaAlunoNaTurma = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(turma.CodigoTurma, codigoAluno));

                    if (matriculaAlunoNaTurma != null && matriculaAlunoNaTurma.Inativo)
                        turmasDoAlunoNoAno.Remove(turma.CodigoTurma);
                }
            }   
        }
    }
}
