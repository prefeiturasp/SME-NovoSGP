using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class VerificaNotasTodosComponentesCurricularesQueryHandler : IRequestHandler<VerificaNotasTodosComponentesCurricularesQuery,bool>
    {
	    private readonly IMediator mediator;

        public VerificaNotasTodosComponentesCurricularesQueryHandler(IMediator mediator)
        {
	        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaNotasTodosComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            long[] conselhosClassesIds;
            string[] turmasCodigos;
            var turmasitinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

            if (request.Turma.DeveVerificarRegraRegulares() || turmasitinerarioEnsinoMedio.Any(a => a.Id == (int)request.Turma.TipoTurma))
            {
                var turmasCodigosParaConsulta = new List<int>();
                turmasCodigosParaConsulta.AddRange(request.Turma.ObterTiposRegularesDiferentes());
                turmasCodigosParaConsulta.AddRange(turmasitinerarioEnsinoMedio.Select(s => s.Id));
                turmasCodigos = await mediator
                    .Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(request.Turma.AnoLetivo, request.AlunoCodigo, turmasCodigosParaConsulta, request.Historico));

                turmasCodigos = turmasCodigos
                    .Concat(new string[] { request.Turma.CodigoTurma }).ToArray();
            }
            else turmasCodigos = new string[] { request.Turma.CodigoTurma };

            conselhosClassesIds = await mediator
                      .Send(new ObterConselhoClasseIdsPorTurmaEBimestreQuery(turmasCodigos, request.Bimestre));


            var notasParaVerificar = new List<NotaConceitoBimestreComponenteDto>();
            if (conselhosClassesIds != null)
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, request.AlunoCodigo,
                                                                 (request.Bimestre ?? 0)));

                    notasParaVerificar.AddRange(notasParaAdicionar);
                }
            }

            if ((request.Bimestre ?? 0) > 0)
                notasParaVerificar.AddRange(await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, request.AlunoCodigo, (request.Bimestre ?? 0))));
            else
            {
                var todasAsNotas = await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, request.AlunoCodigo));

                if (todasAsNotas != null && todasAsNotas.Any())
                    notasParaVerificar.AddRange(todasAsNotas.Where(a => a.Bimestre == null));
            }

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, request.Turma.EnsinoEspecial, request.Turma.TurnoParaComponentesCurriculares);
            var disciplinasDaTurma = await mediator
                .Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));

            // Checa se todas as disciplinas da turma receberam nota
            var disciplinasLancamNota = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome != null);
            foreach (var componenteCurricular in disciplinasLancamNota)
            {
                if (!notasParaVerificar.Any(c => c.ComponenteCurricularCodigo == componenteCurricular.CodigoComponenteCurricular))
                    return false;
            }

            return true;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurmas(string[] turmasCodigo, bool ehEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
	        var componentesTurma = new List<DisciplinaDto>();
	        var usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

	        var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
	        if (componentesCurriculares != null && componentesCurriculares.Any())
		        componentesTurma.AddRange(componentesCurriculares);
	        else throw new NegocioException("Não localizado disciplinas para a turma no EOL!");

	        return componentesTurma;
        }


    }
}