﻿using MediatR;
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
            var periodoConsiderado = periodos?.SingleOrDefault(p => p.PeriodoInicio.Date <= request.DataAula.Date && p.PeriodoFim.Date >= request.DataAula.Date);

            if (periodoConsiderado == null)
                throw new NegocioException("A data da aula está fora dos períodos escolares da turma");

            var alunos = request.Alunos;

            if (alunos.Any())
            {
                var excluirFrequenciaAlunoIds = new List<long>();
                var registroFreqAlunos = (await mediator.Send(new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery(request.DataAula, alunos.ToList(), request.TurmaId))).ToList();
                var periodosEscolaresParaFiltro = new List<long?>() { periodoConsiderado.Id };
                var frequenciaDosAlunos = (await mediator.Send(new ObterFrequenciasPorAlunosTurmaQuery(request.Alunos, periodosEscolaresParaFiltro, request.TurmaId, request.DisciplinaId))).ToList();

                if (registroFreqAlunos.Any())
                {
                    var alunosComFrequencia = registroFreqAlunos.Select(a => a.AlunoCodigo).Distinct().ToList();
                    var registroFrequenciaAgregado = ObterRegistroFrequenciaAgregado(registroFreqAlunos);
                    var totalCompensacoesDisciplinaAlunos = await mediator.Send(new ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery(periodoConsiderado.Bimestre, alunosComFrequencia, request.TurmaId));

                    foreach (var alunoCodigo in alunosComFrequencia)
                    {
                        var totalAulasNaDisciplinaParaAluno = registroFreqAlunos
                            .FirstOrDefault(t => t.AlunoCodigo.Equals(alunoCodigo) && t.ComponenteCurricularId.Equals(request.DisciplinaId))
                            .TotalAulas;

                        var totalAulasParaAluno = registroFreqAlunos
                            .Where(t => t.AlunoCodigo.Equals(alunoCodigo))
                            .Sum(s => s.TotalAulas);

                        if (totalAulasNaDisciplinaParaAluno == 0)
                            excluirFrequenciaAlunoIds.AddRange(frequenciaDosAlunos
                                .Where(w => w.Tipo == TipoFrequenciaAluno.PorDisciplina && w.DisciplinaId.Equals(request.DisciplinaId) && w.CodigoAluno.Equals(alunoCodigo))
                                .Select(s => s.Id));

                        if (totalAulasParaAluno == 0)
                            excluirFrequenciaAlunoIds.AddRange(frequenciaDosAlunos
                                .Where(w => w.Tipo == TipoFrequenciaAluno.Geral && w.CodigoAluno.Equals(alunoCodigo))
                                .Select(s => s.Id));

                        TrataFrequenciaPorDisciplinaAluno(alunoCodigo, totalAulasNaDisciplinaParaAluno, registroFrequenciaAgregado, frequenciaDosAlunos, totalCompensacoesDisciplinaAlunos, turma, request.DisciplinaId, periodoConsiderado);
                        TrataFrequenciaGlobalAluno(alunoCodigo, totalAulasParaAluno, registroFrequenciaAgregado, frequenciaDosAlunos, totalCompensacoesDisciplinaAlunos, request.TurmaId);
                    }

                    VerificaExclusaoFrequenciaConsolidadaMasNaoLancada(registroFreqAlunos, frequenciaDosAlunos, excluirFrequenciaAlunoIds);
                    await TrataPersistencia(frequenciaDosAlunos);
                }

                if (excluirFrequenciaAlunoIds.Any())
                    await ExcluirFrequenciaAluno(excluirFrequenciaAlunoIds);
            }
        }

        public void VerificaExclusaoFrequenciaConsolidadaMasNaoLancada(IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registrosLancados, IEnumerable<FrequenciaAluno> frequenciasConsolidadas, List<long> frequenciasParaExcluir)
        {
            if (frequenciasConsolidadas.Any() && registrosLancados.Any())
            {
                var periodoRegistrosLancados = registrosLancados
                    .Select(rl => rl.PeriodoEscolarId)
                    .Distinct();

                if (!periodoRegistrosLancados.Any())
                    return;

                var frequenciasParaRealizarExclusao = frequenciasConsolidadas
                    .Where(f => periodoRegistrosLancados.Contains(f.PeriodoEscolarId) &&
                                f.Tipo == TipoFrequenciaAluno.PorDisciplina &&
                                !registrosLancados.Any(r => r.AlunoCodigo == f.CodigoAluno && r.ComponenteCurricularId == f.DisciplinaId))
                    .Select(f => f.Id).ToList();

                frequenciasParaExcluir
                    .AddRange(frequenciasParaRealizarExclusao);
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

                var totalAusencias = registroFrequenciaAluno?.TotalAusencias ?? 0;

                if (frequenciaParaTratar == null)
                {
                    var frequenciaFinal = new FrequenciaAluno(
                                 alunoCodigo,
                                 turma.CodigoTurma,
                                 componenteCurricularId,
                                 registroFrequenciaAluno?.PeriodoEscolarId ?? periodoEscolar.Id,
                                 registroFrequenciaAluno?.PeriodoInicio ?? periodoEscolar.PeriodoInicio,
                                 registroFrequenciaAluno?.PeriodoFim ?? periodoEscolar.PeriodoFim,
                                 registroFrequenciaAluno?.Bimestre ?? periodoEscolar.Bimestre,
                                 totalAusencias > totalAulasNaDisciplina ? totalAulasNaDisciplina : totalAusencias,
                                 totalAulasNaDisciplina,
                                 totalAusencias >= totalCompensacoes ? totalCompensacoes : totalAusencias,
                                 TipoFrequenciaAluno.PorDisciplina,
                                 registroFrequenciaAluno?.TotalRemotos ?? 0,
                                 registroFrequenciaAluno?.TotalPresencas ?? totalAulasNaDisciplina);

                    frequenciaDosAlunos.Add(frequenciaFinal);
                }
                else
                {
                    var totalCompensacoesDisciplinas = totalCompensacoesDisciplinaAluno?.Compensacoes ?? 0;
                    frequenciaParaTratar
                        .DefinirFrequencia(totalAusencias > totalAulasNaDisciplina ? totalAulasNaDisciplina : totalAusencias,
                                           totalAulasNaDisciplina,
                                           totalAusencias >= totalCompensacoes ? totalCompensacoes : totalAusencias,
                                           TipoFrequenciaAluno.PorDisciplina,
                                           registroFrequenciaAluno?.TotalRemotos ?? 0,
                                           registroFrequenciaAluno?.TotalPresencas ?? totalAulasNaDisciplina);
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

                var totalAusenciasDisciplina = registroFrequenciaAlunos.Where(a => a.AlunoCodigo == alunoCodigo &&
                    compensacoesDisciplinasAlunos.Any(b => b.ComponenteCurricularId == a.ComponenteCurricularId)).Select(x => (x.ComponenteCurricularId, x.TotalAusencias)).ToList();

                var totalCompensacoesDisciplina = compensacoesDisciplinasAlunos
                    .Where(a => a.AlunoCodigo == alunoCodigo).Select(x => (x.ComponenteCurricularId, x.Compensacoes)).ToList();

                if (totalCompensacoesDisciplina.Any())
                {
                    totalCompensacoesDoAlunoGeral = totalAusenciasDisciplina.Sum(b => b.TotalAusencias >= totalCompensacoesDisciplina.FirstOrDefault(x => x.ComponenteCurricularId == b.ComponenteCurricularId).Compensacoes ?
                        totalCompensacoesDisciplina.FirstOrDefault(x => x.ComponenteCurricularId == b.ComponenteCurricularId).Compensacoes : b.TotalAusencias);
                }

                var frequenciaParaTratar = frequenciaDosAlunos.OrderByDescending(a => a.Id)
                    .FirstOrDefault(a => a.CodigoAluno == alunoCodigo && string.IsNullOrEmpty(a.DisciplinaId) && a.Bimestre == registroFrequenciaAluno.Bimestre);

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
                                 registroFrequenciaAluno.TotalAusencias > totalAulasDaTurmaGeral ? totalAulasDaTurmaGeral : registroFrequenciaAluno.TotalAusencias,
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
                        .DefinirFrequencia(registroFrequenciaAluno.TotalAusencias > totalAulasDaTurmaGeral ? totalAulasDaTurmaGeral : registroFrequenciaAluno.TotalAusencias,
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