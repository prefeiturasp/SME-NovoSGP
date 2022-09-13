using MediatR;
using Polly;
using Polly.Registry;
using SME.SGP.Infra;
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

            } catch(Exception e)
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
            if (request.ConsideraTodosAlunos)
            {
                var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaId));
                if (alunosDaTurma == null || !alunosDaTurma.Any())
                    throw new NegocioException($"Não localizados alunos para turma [{request.TurmaId}] no EOL");

                request.Alunos = request.Alunos.Union(alunosDaTurma.Select(a => a.CodigoAluno)).ToArray();
            }

            var excluirFrequenciaAlunoIds = new List<long>();

            var registroFreqAlunos = (await mediator.Send(new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery(request.DataAula, request.Alunos, request.TurmaId))).ToList();
            
            var periodosEscolaresParaFiltro = registroFreqAlunos.Select(a => a.PeriodoEscolarId).Distinct().ToList();

            List<FrequenciaAluno> frequenciaDosAlunos = (await mediator.Send(new ObterFrequenciasPorAlunosTurmaQuery(request.Alunos, periodosEscolaresParaFiltro, request.TurmaId, request.DisciplinaId))).ToList();

            if (registroFreqAlunos.Any())
            {
                var totalAulasDaDisciplina = await mediator.Send(new ObterTotalAulasPorDisciplinaETurmaQuery(request.DataAula, request.DisciplinaId, request.TurmaId));
                var totalAulasDaTurmaGeral = await mediator.Send(new ObterTotalAulasPorDisciplinaETurmaQuery(request.DataAula, string.Empty, request.TurmaId));
                var bimestre = registroFreqAlunos.Select(a => a.Bimestre).First();

                if (totalAulasDaDisciplina == 0)
                    excluirFrequenciaAlunoIds.AddRange(frequenciaDosAlunos.Where(w => w.DisciplinaId.Equals(request.DisciplinaId)).Select(s => s.Id));

                VerificaFrequenciasDuplicadas(frequenciaDosAlunos, request.DisciplinaId, excluirFrequenciaAlunoIds, bimestre);

                var alunosComFrequencia = registroFreqAlunos.Select(a => a.AlunoCodigo).Distinct().ToList();
                
                var registroFrequenciaAgregado = ObterRegistroFrequenciaAgregado(registroFreqAlunos);

                var totalCompensacoesDisciplinaAlunos = await mediator.Send(new ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery(bimestre, alunosComFrequencia, request.TurmaId));

                foreach (var codigoAluno in alunosComFrequencia)
                {
                    var ausenciasDoAluno = alunosComFrequencia.Where(a => a == codigoAluno).ToList();

                    TrataFrequenciaAlunoComponente(request, frequenciaDosAlunos, totalAulasDaDisciplina, totalCompensacoesDisciplinaAlunos, codigoAluno, registroFrequenciaAgregado);
                    TrataFrequenciaGlobalAluno(codigoAluno, totalAulasDaTurmaGeral, registroFrequenciaAgregado, frequenciaDosAlunos, totalCompensacoesDisciplinaAlunos, request.TurmaId);
                }

                VerificaExclusaoFrequenciaConsolidadaMasNaoLancada(registroFreqAlunos, frequenciaDosAlunos, excluirFrequenciaAlunoIds);
            }
            await ExcluirFrequenciaAluno(excluirFrequenciaAlunoIds);
            await TrataPersistencia(frequenciaDosAlunos);
        }

        public void VerificaExclusaoFrequenciaConsolidadaMasNaoLancada(IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registrosLancados, IEnumerable<FrequenciaAluno> frequenciasConsolidadas, List<long> frequenciasParaExcluir)
        {
            if (frequenciasConsolidadas.Any() && registrosLancados.Any())
            {
                var frequenciasParaRealizarExclusao = frequenciasConsolidadas.Where(f => !registrosLancados.Any(r => r.AlunoCodigo == f.CodigoAluno
                                                                                && r.Bimestre == f.Bimestre && r.PeriodoEscolarId == f.PeriodoEscolarId))
                                                                             .Select(f => f.Id).ToList();
                frequenciasParaExcluir.AddRange(frequenciasParaRealizarExclusao);
            }  
        }

        private void VerificaFrequenciasDuplicadas(IEnumerable<FrequenciaAluno> frequenciasAlunos, string disciplinaId, List<long> frequenciaAlunosIdsParaExcluir, int bimestre)
        {
            var frequenciaAgrupada = frequenciasAlunos.GroupBy(f => f.CodigoAluno);

            foreach(var frequencia in frequenciaAgrupada)
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

        private void TrataFrequenciaAlunoComponente(CalcularFrequenciaPorTurmaCommand request,
                                                    List<FrequenciaAluno> frequenciaDosAlunos,
                                                    int totalAulasNaDisciplina,
                                                    IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> totalCompensacoesDisciplinaAlunos,
                                                    string codigoAluno,
                                                    List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAluno)
        {

            var totalAulasNaDisciplinaPorAluno = registroFrequenciaAluno
                .Where(aluno => aluno.AlunoCodigo == codigoAluno
                            && aluno.ComponenteCurricularId == request.DisciplinaId)
                .Aggregate(0,
                    (total, frequencia) =>
                        total + (frequencia.TotalPresencas + frequencia.TotalAusencias + frequencia.TotalRemotos));

            TrataFrequenciaPorDisciplinaAluno(codigoAluno,
                                              totalAulasNaDisciplinaPorAluno == 0 ? totalAulasNaDisciplina : totalAulasNaDisciplinaPorAluno,
                                              registroFrequenciaAluno,
                                              frequenciaDosAlunos,
                                              totalCompensacoesDisciplinaAlunos,
                                              request.TurmaId,
                                              request.DisciplinaId);
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
                {
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
                }
            }
        }

        private void TrataFrequenciaPorDisciplinaAluno(string alunoCodigo, int totalAulasNaDisciplina, List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos,
            List<FrequenciaAluno> frequenciaDosAlunos, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
            string turmaId, string componenteCurricularId)
        {
            if (registroFrequenciaAlunos.Any(a => a.ComponenteCurricularId == componenteCurricularId))
            {
                var registroFrequenciaAluno = registroFrequenciaAlunos.FirstOrDefault(a => a.AlunoCodigo == alunoCodigo && a.ComponenteCurricularId == componenteCurricularId);

                var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => a.CodigoAluno == alunoCodigo && a.DisciplinaId == componenteCurricularId);
                var totalCompensacoes = 0;

                var totalCompensacoesDisciplinaAluno = compensacoesDisciplinasAlunos.FirstOrDefault(a => a.AlunoCodigo == alunoCodigo && a.ComponenteCurricularId == componenteCurricularId);
                if (totalCompensacoesDisciplinaAluno != null)
                    totalCompensacoes = totalCompensacoesDisciplinaAluno.Compensacoes;

                if (frequenciaParaTratar == null)
                {
                    var frequenciaFinal = new FrequenciaAluno
                             (
                                 alunoCodigo,
                                 turmaId,
                                 componenteCurricularId,
                                 registroFrequenciaAluno.PeriodoEscolarId,
                                 registroFrequenciaAluno.PeriodoInicio,
                                 registroFrequenciaAluno.PeriodoFim,
                                 registroFrequenciaAluno.Bimestre,
                                 registroFrequenciaAluno.TotalAusencias,
                                 totalAulasNaDisciplina,
                                 totalCompensacoes,
                                 TipoFrequenciaAluno.PorDisciplina,
                                 registroFrequenciaAluno.TotalRemotos,
                                 registroFrequenciaAluno.TotalPresencas);

                    frequenciaDosAlunos.Add(frequenciaFinal);
                }
                else
                {
                    frequenciaParaTratar.DefinirFrequencia(registroFrequenciaAluno.TotalAusencias, totalAulasNaDisciplina, (totalCompensacoesDisciplinaAluno?.Compensacoes ?? 0), TipoFrequenciaAluno.PorDisciplina, registroFrequenciaAluno.TotalRemotos, registroFrequenciaAluno.TotalPresencas);
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

                var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => a.CodigoAluno == alunoCodigo && string.IsNullOrEmpty(a.DisciplinaId));
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
                                 totalCompensacoesDoAlunoGeral,
                                 TipoFrequenciaAluno.Geral,
                                 registroFrequenciaAluno.TotalRemotos,
                                 registroFrequenciaAluno.TotalPresencas);

                    frequenciaDosAlunos.Add(frequenciaGlobal);
                }
                else
                    frequenciaParaTratar.DefinirFrequencia(registroFrequenciaAluno.TotalAusencias, totalAulasDaTurmaGeral, totalCompensacoesDoAlunoGeral, TipoFrequenciaAluno.Geral, registroFrequenciaAluno.TotalRemotos, registroFrequenciaAluno.TotalPresencas);
            }
        }

    }
}