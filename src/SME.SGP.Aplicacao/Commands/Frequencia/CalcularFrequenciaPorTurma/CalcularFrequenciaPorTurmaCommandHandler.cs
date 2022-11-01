using MediatR;
using Polly;
using Polly.Registry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<CalcularFrequenciaPorTurmaCommand, bool>
    {
        public readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IMediator mediator;
        private readonly IAsyncPolicy policy;

        public CalcularFrequenciaPorTurmaCommandHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                                        IMediator mediator,
                                                        IReadOnlyPolicyRegistry<string> registry)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task<bool> Handle(CalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await CalcularFrequenciaPorTurma(request);

            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Erro ao calcular Frequência da Turma: " + e.Message,
                                                                  LogNivel.Critico,
                                                                  LogContexto.Frequencia,
                                                                  JsonSerializer.Serialize(request),
                                                                  rastreamento: e.StackTrace,
                                                                  excecaoInterna: e.InnerException?.ToString()));
                throw;
            }

            return true;
        }

        private async Task CalcularFrequenciaPorTurma(CalcularFrequenciaPorTurmaCommand request)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaId));
            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));
            var periodoConsiderado = periodos?.FirstOrDefault(p => p.PeriodoInicio.Date <= request.DataAula.Date && p.PeriodoFim.Date >= request.DataAula.Date);

            if (periodoConsiderado == null)
                throw new NegocioException("A data da aula está fora dos períodos escolares da turma");

            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaId, true));
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException($"Não localizados alunos para turma [{request.TurmaId}] no EOL");

            if (request.ConsideraTodosAlunos)
                request.Alunos = request.Alunos.Union(alunosDaTurma.Select(a => a.CodigoAluno)).ToArray();

            var excluirFrequenciaAlunoIds = new List<long>();
            var registroFreqAlunos = (await mediator.Send(new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery(request.DataAula, request.Alunos, request.TurmaId))).ToList();
            var periodosEscolaresParaFiltro = periodos.Select(p => (long?)p.Id);
            var frequenciaDosAlunos = (await mediator.Send(new ObterFrequenciasPorAlunosTurmaQuery(request.Alunos, periodosEscolaresParaFiltro, request.TurmaId, request.DisciplinaId))).ToList();
            var totalAulasDaDisciplina = await mediator.Send(new ObterTotalAulasPorDisciplinaETurmaQuery(request.DataAula, request.DisciplinaId, turmasId: request.TurmaId));

            if (totalAulasDaDisciplina == 0)
                excluirFrequenciaAlunoIds.AddRange(frequenciaDosAlunos.Where(w => w.DisciplinaId.Equals(request.DisciplinaId)).Select(s => s.Id));

            VerificaFrequenciasDuplicadas(frequenciaDosAlunos, request.DisciplinaId, excluirFrequenciaAlunoIds, periodoConsiderado.Bimestre);

            var alunosComFrequencia = registroFreqAlunos.Select(a => a.AlunoCodigo).Distinct().ToList();
            var registroFrequenciaAgregado = ObterRegistroFrequenciaAgregado(registroFreqAlunos);
            var totalCompensacoesDisciplinaAlunos = await mediator.Send(new ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery(periodoConsiderado.Bimestre, alunosComFrequencia, request.TurmaId));

            foreach (var codigoAluno in alunosComFrequencia)
            {
                var dadosMatriculaAluno = alunosDaTurma.SingleOrDefault(a => a.CodigoAluno.Equals(codigoAluno));

                if (dadosMatriculaAluno == null)
                    continue;

                var totalAulasNaDisciplinaParaAluno = await mediator
                    .Send(new ObterTotalAulasPorDisciplinaETurmaQuery(request.DataAula, request.DisciplinaId, dadosMatriculaAluno.DataMatricula, dadosMatriculaAluno.EstaInativo(request.DataAula) ? dadosMatriculaAluno.DataSituacao : null, request.TurmaId));

                if (totalAulasNaDisciplinaParaAluno == 0)
                    excluirFrequenciaAlunoIds.AddRange(frequenciaDosAlunos.Where(w => w.DisciplinaId.Equals(request.DisciplinaId) && w.CodigoAluno.Equals(codigoAluno)).Select(s => s.Id));

                var totalAulasParaAluno = await mediator
                    .Send(new ObterTotalAulasPorDisciplinaETurmaQuery(request.DataAula, string.Empty, dadosMatriculaAluno.DataMatricula, dadosMatriculaAluno.EstaInativo(request.DataAula) ? dadosMatriculaAluno.DataSituacao : null, request.TurmaId));

                var ausenciasDoAluno = alunosComFrequencia.Where(a => a == codigoAluno).ToList();

                TrataFrequenciaPorDisciplinaAluno(codigoAluno, totalAulasNaDisciplinaParaAluno, registroFrequenciaAgregado, frequenciaDosAlunos, totalCompensacoesDisciplinaAlunos, turma, request.DisciplinaId, periodoConsiderado);
                TrataFrequenciaGlobalAluno(codigoAluno, totalAulasParaAluno, registroFrequenciaAgregado, frequenciaDosAlunos, totalCompensacoesDisciplinaAlunos, request.TurmaId);
            }

            VerificaExclusaoFrequenciaConsolidadaMasNaoLancada(registroFreqAlunos, frequenciaDosAlunos, excluirFrequenciaAlunoIds);
            await ExcluirFrequenciaAluno(excluirFrequenciaAlunoIds);
            await TrataPersistencia(frequenciaDosAlunos);
        }

        public void VerificaExclusaoFrequenciaConsolidadaMasNaoLancada(IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registrosLancados, IEnumerable<FrequenciaAluno> frequenciasConsolidadas, List<long> frequenciasParaExcluir)
        {
            if (frequenciasConsolidadas.Any() && registrosLancados.Any())
            {
                var periodoRegistrosLancados = registrosLancados
                    .Select(rl => rl.PeriodoEscolarId)
                    .Distinct().SingleOrDefault() ?? 0;

                if (periodoRegistrosLancados == 0)
                    return;

                var frequenciasParaRealizarExclusao = frequenciasConsolidadas
                    .Where(f => f.PeriodoEscolarId == periodoRegistrosLancados && 
                                f.Tipo == TipoFrequenciaAluno.PorDisciplina && 
                                !registrosLancados.Any(r => r.AlunoCodigo == f.CodigoAluno && r.ComponenteCurricularId == f.DisciplinaId))
                    .Select(f => f.Id).ToList();

                frequenciasParaExcluir
                    .AddRange(frequenciasParaRealizarExclusao);
            }
        }

        private void VerificaFrequenciasDuplicadas(IEnumerable<FrequenciaAluno> frequenciasAlunos, string disciplinaId, List<long> frequenciaAlunosIdsParaExcluir, int bimestre)
        {
            var frequenciaAgrupada = frequenciasAlunos.GroupBy(f => f.CodigoAluno);

            foreach (var frequencia in frequenciaAgrupada)
            {
                var frequenciasDuplicadas = frequencia.Where(f => f.DisciplinaId == disciplinaId && f.Bimestre == bimestre).Select(f => f.Id);
                if (frequenciasDuplicadas.Count() > 1)
                {
                    frequenciaAlunosIdsParaExcluir.AddRange(frequenciasDuplicadas);
                    frequenciaAlunosIdsParaExcluir.Remove(frequenciasDuplicadas.FirstOrDefault()); // mantém somente uma frequência na lista para fazer a alteração na base com os dados atualizados
                }
            }
        }

        private async Task ExcluirFrequenciaAluno(List<long> excluirFrequenciaAlunoIds)
        {
            if (excluirFrequenciaAlunoIds.Any())
                await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverVariosAsync(excluirFrequenciaAlunoIds.ToArray());
        }

        private List<RegistroFrequenciaPorDisciplinaAlunoDto> ObterRegistroFrequenciaAgregado(List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFreqAlunos)
        {
            return registroFreqAlunos.GroupBy(g => new { g.PeriodoEscolarId, g.PeriodoInicio, g.PeriodoFim, g.Bimestre, g.AlunoCodigo, g.ComponenteCurricularId }, (key, group) =>
            new
            {
                key.PeriodoEscolarId,
                key.PeriodoInicio,
                key.PeriodoFim,
                key.Bimestre,
                key.AlunoCodigo,
                key.ComponenteCurricularId,
                TotalPresencas = group.Sum(s => s.TotalPresencas),
                TotalAusencias = group.Sum(s => s.TotalAusencias),
                TotalRemotos = group.Sum(s => s.TotalRemotos)
            }).Select(s => new RegistroFrequenciaPorDisciplinaAlunoDto()
            {
                PeriodoEscolarId = s.PeriodoEscolarId,
                PeriodoInicio = s.PeriodoInicio,
                PeriodoFim = s.PeriodoFim,
                Bimestre = s.Bimestre,
                AlunoCodigo = s.AlunoCodigo,
                ComponenteCurricularId = s.ComponenteCurricularId,
                TotalPresencas = s.TotalPresencas,
                TotalAusencias = s.TotalAusencias,
                TotalRemotos = s.TotalRemotos
            }).ToList();
        }

        private async Task TrataPersistencia(List<FrequenciaAluno> frequenciasParaPersistir)
        {
            await policy.ExecuteAsync(() => Persistir(frequenciasParaPersistir));
        }

        private async Task Persistir(List<FrequenciaAluno> frequenciasParaPersistir)
        {
            if (frequenciasParaPersistir != null && frequenciasParaPersistir.Any())
            {
                if (frequenciasParaPersistir.Any(a => a.FrequenciaNegativa()))
                    throw new Exception($"Erro ao calcular frequencia da turma {frequenciasParaPersistir.First().TurmaId} : Número de ausências maior do que o número de aulas");

                foreach (var frequenciaAluno in frequenciasParaPersistir)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
            }
        }

        private void TrataFrequenciaPorDisciplinaAluno(string alunoCodigo, int totalAulasNaDisciplina,
                                                       List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos,
                                                       List<FrequenciaAluno> frequenciaDosAlunos,
                                                       IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
                                                       Turma turma, string componenteCurricularId, PeriodoEscolar periodoEscolar)
        {
            if (registroFrequenciaAlunos.Any(a => a.ComponenteCurricularId == componenteCurricularId) || totalAulasNaDisciplina > 0)
            {
                var registroFrequenciaAluno = registroFrequenciaAlunos
                    .FirstOrDefault(a => a.AlunoCodigo == alunoCodigo && a.ComponenteCurricularId == componenteCurricularId);

                var frequenciaParaTratar = frequenciaDosAlunos
                    .FirstOrDefault(a => a.CodigoAluno == alunoCodigo && a.DisciplinaId == componenteCurricularId && a.Bimestre == periodoEscolar.Bimestre);

                var totalCompensacoes = 0;
                var totalCompensacoesDisciplinaAluno = compensacoesDisciplinasAlunos
                    .FirstOrDefault(a => a.AlunoCodigo == alunoCodigo && a.ComponenteCurricularId == componenteCurricularId);

                if (totalCompensacoesDisciplinaAluno != null)
                    totalCompensacoes = totalCompensacoesDisciplinaAluno.Compensacoes;

                var totalPresencas = registroFrequenciaAluno?.TotalPresencas ?? totalAulasNaDisciplina;
                totalPresencas = totalPresencas > totalAulasNaDisciplina ? totalAulasNaDisciplina : totalPresencas;

                var totalAusencias = registroFrequenciaAluno?.TotalAusencias ?? 0;

                if (frequenciaParaTratar == null)
                {
                    if (registroFrequenciaAluno != null)
                    {
                        var frequenciaFinal = new FrequenciaAluno
                             (
                                 alunoCodigo,
                                 turma.CodigoTurma,
                                 componenteCurricularId,
                                 registroFrequenciaAluno.PeriodoEscolarId,
                                 registroFrequenciaAluno.PeriodoInicio,
                                 registroFrequenciaAluno.PeriodoFim,
                                 registroFrequenciaAluno.Bimestre,
                                 totalAusencias > totalAulasNaDisciplina ? totalAulasNaDisciplina : totalAusencias,
                                 totalAulasNaDisciplina,
                                 totalCompensacoes,
                                 TipoFrequenciaAluno.PorDisciplina,
                                 registroFrequenciaAluno?.TotalRemotos ?? 0,
                                 totalPresencas);

                        frequenciaDosAlunos.Add(frequenciaFinal);
                    }
                    else
                    {
                        frequenciaParaTratar
                            .DefinirFrequencia(totalAusencias > totalAulasNaDisciplina ? totalAulasNaDisciplina : totalAusencias,
                                               totalAulasNaDisciplina,
                                               totalCompensacoesDisciplinaAluno?.Compensacoes ?? 0,
                                               TipoFrequenciaAluno.PorDisciplina,
                                               registroFrequenciaAluno?.TotalRemotos ?? 0,
                                               totalPresencas);
                    }
                }
            }
        }

        private void TrataFrequenciaGlobalAluno(string alunoCodigo,
                                                int totalAulasDaTurmaGeral,
                                                List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos,
                                                List<FrequenciaAluno> frequenciaDosAlunos,
                                                IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
                                                string turmaId)
        {
            if (registroFrequenciaAlunos.Any())
            {
                int totalCompensacoesDoAlunoGeral = 0;

                var registroFrequenciaAluno = registroFrequenciaAlunos.Where(a => a.AlunoCodigo == alunoCodigo)
                    .GroupBy(g => new { g.PeriodoEscolarId, g.PeriodoInicio, g.PeriodoFim, g.Bimestre, g.AlunoCodigo }, (key, group) =>
                new
                {
                    key.PeriodoEscolarId,
                    key.PeriodoInicio,
                    key.PeriodoFim,
                    key.Bimestre,
                    key.AlunoCodigo,
                    TotalPresencas = group.Sum(s => s.TotalPresencas),
                    TotalAusencias = group.Sum(s => s.TotalAusencias),
                    TotalRemotos = group.Sum(s => s.TotalRemotos)
                }).Select(s => new RegistroFrequenciaPorDisciplinaAlunoDto()
                {
                    PeriodoEscolarId = s.PeriodoEscolarId,
                    PeriodoInicio = s.PeriodoInicio,
                    PeriodoFim = s.PeriodoFim,
                    Bimestre = s.Bimestre,
                    AlunoCodigo = s.AlunoCodigo,
                    TotalPresencas = s.TotalPresencas,
                    TotalAusencias = s.TotalAusencias,
                    TotalRemotos = s.TotalRemotos
                }).FirstOrDefault();

                var totaisDoAluno = compensacoesDisciplinasAlunos.Where(a => a.AlunoCodigo == alunoCodigo).ToList();
                if (totaisDoAluno.Any())
                    totalCompensacoesDoAlunoGeral = totaisDoAluno.Sum(a => a.Compensacoes);

                var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => a.CodigoAluno == alunoCodigo && string.IsNullOrEmpty(a.DisciplinaId) && a.Bimestre == registroFrequenciaAluno.Bimestre);
                if (frequenciaParaTratar == null)
                {
                    var frequenciaGlobal = new FrequenciaAluno(
                                 alunoCodigo,
                                 turmaId,
                                 string.Empty,
                                 registroFrequenciaAluno.PeriodoEscolarId,
                                 registroFrequenciaAluno.PeriodoInicio,
                                 registroFrequenciaAluno.PeriodoFim,
                                 registroFrequenciaAluno.Bimestre,
                                 registroFrequenciaAluno.TotalAusencias,
                                 totalAulasDaTurmaGeral,
                                 registroFrequenciaAluno.TotalAusencias >= totalCompensacoesDoAlunoGeral ? totalCompensacoesDoAlunoGeral : registroFrequenciaAluno.TotalAusencias,
                                 TipoFrequenciaAluno.Geral,
                                 registroFrequenciaAluno.TotalRemotos,
                                 registroFrequenciaAluno.TotalPresencas);

                    frequenciaDosAlunos.Add(frequenciaGlobal);
                }
                else
                {
                    frequenciaParaTratar
                        .DefinirFrequencia(registroFrequenciaAluno.TotalAusencias,
                                           totalAulasDaTurmaGeral,
                                           registroFrequenciaAluno.TotalAusencias >= totalCompensacoesDoAlunoGeral ? totalCompensacoesDoAlunoGeral : registroFrequenciaAluno.TotalAusencias,
                                           TipoFrequenciaAluno.Geral,
                                           registroFrequenciaAluno.TotalRemotos,
                                           registroFrequenciaAluno.TotalPresencas);
                }
            }
        }

    }
}