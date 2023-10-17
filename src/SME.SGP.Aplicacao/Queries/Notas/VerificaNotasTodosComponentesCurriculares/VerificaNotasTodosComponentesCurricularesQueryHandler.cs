using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class VerificaNotasTodosComponentesCurricularesQueryHandler : IRequestHandler<VerificaNotasTodosComponentesCurricularesQuery, bool>
    {
        private readonly IMediator mediator;
        private const string PRIMEIRO_ANO_EM = "1";

        public VerificaNotasTodosComponentesCurricularesQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaNotasTodosComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            string[] turmasCodigos;
            
            var turmasItinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance, cancellationToken)).ToList();

            if (request.Turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)request.Turma.TipoTurma))
            {
                var tiposTurmas = new List<int> { (int) request.Turma.TipoTurma };
                
                tiposTurmas.AddRange(request.Turma.ObterTiposRegularesDiferentes());
                tiposTurmas.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c=> tiposTurmas.All(x=> x != c)));
                
                var turmasCodigosEol = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(request.Turma.AnoLetivo, request.AlunoCodigo, tiposTurmas,
                   dataReferencia: request.PeriodoEscolar?.PeriodoFim, semestre:request.Turma.EhEJA() ? request.Turma.Semestre : null), cancellationToken);

                if (request.Historico.HasValue && request.Historico.Value)
                {
                    var turmasCodigosHistorico = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigosEol), cancellationToken);

                    if (turmasCodigosHistorico.Any(x => x.EhTurmaHistorica))
                    {
                        turmasCodigos = turmasCodigosEol;
                        turmasCodigos = !turmasCodigosEol.Contains(request.Turma.CodigoTurma) ? turmasCodigos.Concat(new[] { request.Turma.CodigoTurma }).ToArray() : turmasCodigosEol;
                    }
                    else
                        turmasCodigos = new[] { request.Turma.CodigoTurma };
                }
                else
                    turmasCodigos = !turmasCodigosEol.Contains(request.Turma.CodigoTurma) ? turmasCodigosEol.Concat(new[] { request.Turma.CodigoTurma }).ToArray() : turmasCodigosEol;

                if (turmasCodigos.Length > 0)
                {
                    var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos.ToArray()));

                    if (turmas.Select(t => t.TipoTurma).Distinct().Count() == 1 && request.Turma.ModalidadeCodigo != Modalidade.Medio)
                        turmasCodigos = new string[] { request.Turma.CodigoTurma };
                    else if (ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(turmas, request.Turma))
                        turmasCodigos = new string[] { request.Turma.CodigoTurma };
                }
            }
            else 
                turmasCodigos = new[] { request.Turma.CodigoTurma };

            var conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEBimestreQuery(turmasCodigos, request.Bimestre), cancellationToken);

            var notasParaVerificar = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds.NaoEhNulo())
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, request.AlunoCodigo, request.Bimestre ?? 0), cancellationToken);
                    notasParaVerificar.AddRange(notasParaAdicionar);
                }
            }

            var todasAsNotas = (await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigos, request.AlunoCodigo), cancellationToken)).ToList();

            if (todasAsNotas.Any())
            {
                var bimestre = request.Bimestre == 0 ? null : request.Bimestre;

                notasParaVerificar.AddRange(todasAsNotas.Where(a => a.Bimestre == bimestre));
            }
                
            turmasCodigos = await DefinirTurmasConsideradasDeAcordoComMatricula(request.AlunoCodigo, request.PeriodoEscolar, turmasCodigos);

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, request.Turma.EnsinoEspecial, request.Turma.TurnoParaComponentesCurriculares);
            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray(), codigoTurma: request.Turma.CodigoTurma), cancellationToken);

            // Checa se todas as disciplinas da turma receberam nota
            var disciplinasLancamNota = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome.NaoEhNulo());
            return disciplinasLancamNota.All(componenteCurricular => notasParaVerificar.Any(c => c.ComponenteCurricularCodigo == componenteCurricular.CodigoComponenteCurricular));
        }

        private async Task<string[]> DefinirTurmasConsideradasDeAcordoComMatricula(string alunoCodigo, Dominio.PeriodoEscolar periodoEscolar, string[] turmasCodigos)
        {
            if (periodoEscolar.NaoEhNulo())
            {
                var codigosTurmaVerificacao = new string[turmasCodigos.Length];
                turmasCodigos.CopyTo(codigosTurmaVerificacao, 0);
                var listaCodigosConsiderados = new List<string>();

                foreach (var ct in codigosTurmaVerificacao)
                {
                    var dadosMatricula = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(ct, alunoCodigo));
                    if (dadosMatricula.NaoEhNulo() && dadosMatricula.Any() && dadosMatricula.OrderBy(dm => dm.DataSituacao).First().DataMatricula.Date <= periodoEscolar.PeriodoFim.Date)
                        listaCodigosConsiderados.Add(ct);
                }

                if (listaCodigosConsiderados.Any())
                    turmasCodigos = listaCodigosConsiderados.ToArray();
            }

            return turmasCodigos;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurmas(string[] turmasCodigo, bool ehEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
            var componentesTurma = new List<DisciplinaDto>();
            var usuarioAtual = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
            if (componentesCurriculares.NaoEhNulo() && componentesCurriculares.Any())
                componentesTurma.AddRange(componentesCurriculares);
            else throw new NegocioException(MensagemNegocioEOL.NAO_LOCALIZADO_DISCIPLINAS_TURMA_EOL);

            return componentesTurma;
        }

        private bool ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(IEnumerable<Turma> turmasAluno, Turma turmaFiltro)
             => turmasAluno.Select(t => t.TipoTurma).Distinct().Count() == 1 && turmaFiltro.ModalidadeCodigo == Modalidade.Medio && (turmaFiltro.AnoLetivo < 2021 || turmaFiltro.Ano == PRIMEIRO_ANO_EM);

    }
}