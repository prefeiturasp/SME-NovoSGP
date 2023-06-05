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

        public VerificaNotasTodosComponentesCurricularesQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaNotasTodosComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            string[] turmasCodigos;

            var turmasItinerarioEnsinoMedio = (await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery(), cancellationToken)).ToList();

            if (request.Turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)request.Turma.TipoTurma))
            {
                var tiposTurmas = new List<int> { (int) request.Turma.TipoTurma };
                
                tiposTurmas.AddRange(request.Turma.ObterTiposRegularesDiferentes());
                tiposTurmas.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c=> tiposTurmas.All(x=> x != c)));
                
                var turmasCodigosEol = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(request.Turma.AnoLetivo, request.AlunoCodigo, tiposTurmas
                                                            , request.Historico), cancellationToken);

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
            }
            else
                turmasCodigos = new[] { request.Turma.CodigoTurma };

            var conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEBimestreQuery(turmasCodigos, request.Bimestre), cancellationToken);

            var notasParaVerificar = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds != null)
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
                
            turmasCodigos = DefinirTurmasConsideradasDeAcordoComMatricula(request.AlunoCodigo, request.PeriodoEscolar, turmasCodigos);

            var componentesCurriculares = await ObterComponentesTurmas(turmasCodigos, request.Turma.EnsinoEspecial, request.Turma.TurnoParaComponentesCurriculares);
            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(componentesCurriculares.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray(), codigoTurma: request.Turma.CodigoTurma), cancellationToken);

            // Checa se todas as disciplinas da turma receberam nota
            var disciplinasLancamNota = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome != null);
            return disciplinasLancamNota.All(componenteCurricular => notasParaVerificar.Any(c => c.ComponenteCurricularCodigo == componenteCurricular.CodigoComponenteCurricular));
        }

        private string[] DefinirTurmasConsideradasDeAcordoComMatricula(string alunoCodigo, Dominio.PeriodoEscolar periodoEscolar, string[] turmasCodigos)
        {
            if (periodoEscolar != null)
            {
                var codigosTurmaVerificacao = new string[turmasCodigos.Length];
                turmasCodigos.CopyTo(codigosTurmaVerificacao, 0);
                var listaCodigosConsiderados = new List<string>();

                codigosTurmaVerificacao.ToList().ForEach(ct =>
                {
                    var dadosMatricula = mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(ct, alunoCodigo)).Result;
                    if (dadosMatricula != null && dadosMatricula.Any() && dadosMatricula.OrderBy(dm => dm.DataSituacao).First().DataMatricula.Date <= periodoEscolar.PeriodoFim.Date)
                        listaCodigosConsiderados.Add(ct);
                });

                if (listaCodigosConsiderados.Any())
                    turmasCodigos = listaCodigosConsiderados.ToArray();
            }

            return turmasCodigos;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurmas(string[] turmasCodigo, bool ehEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
            var componentesTurma = new List<DisciplinaDto>();
            var usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
            if (componentesCurriculares != null && componentesCurriculares.Any())
                componentesTurma.AddRange(componentesCurriculares);
            else throw new NegocioException(MensagemNegocioEOL.NAO_LOCALIZADO_DISCIPLINAS_TURMA_EOL);

            return componentesTurma;
        }


    }
}