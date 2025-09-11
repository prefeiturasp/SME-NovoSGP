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
        private Turma turma;
        private PeriodoEscolar periodoConsiderado;

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
            var professor = await ObterRfProfessorLogado(request);
            turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaId));
            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));
            periodoConsiderado = ObterPeriodoVigenteDataAula(periodos, request.DataAula.Date);
            var componentesRegenciaAulasAutomaticas = await mediator.Send(new ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQuery(turma.ModalidadeCodigo));
            var alunos = request.Alunos;

            if (alunos.Any())
            {
                var excluirFrequenciaAlunoIds = new List<long>();
                var disciplinasIdsConsideradas = new List<string>() { request.DisciplinaId };

                var registroFreqAlunos = await mediator.Send(new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery(request.DataAula, alunos, turmasId: request.TurmaId));
                var periodosEscolaresParaFiltro = periodos.Select(p => (long?)p.Id);
                var frequenciaDosAlunos = (await mediator.Send(new ObterFrequenciasPorAlunosTurmaQuery(request.Alunos, periodoConsiderado.Id, request.TurmaId, disciplinasIdsConsideradas.ToArray()))).ToList();

                VerificaFrequenciasReplicadasIndevidamente(frequenciaDosAlunos, disciplinasIdsConsideradas.ToArray(), excluirFrequenciaAlunoIds, periodoConsiderado.Bimestre);

                if (registroFreqAlunos.Any())
                {
                    var alunosComFrequencia = registroFreqAlunos.Select(a => a.AlunoCodigo).Distinct().ToList();
                    var registroFrequenciaAgregado = ObterRegistroFrequenciaAgregado(registroFreqAlunos);
                    var totalCompensacoesDisciplinaAlunos = await mediator.Send(new ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery(periodoConsiderado.Bimestre, alunosComFrequencia, request.TurmaId));

                    foreach (var codigoAluno in alunosComFrequencia)
                    {
                        var totalAulasNaDisciplinaParaAluno = ObterTotalAulasAlunoNaDisciplina(registroFreqAlunos, codigoAluno, disciplinasIdsConsideradas);
                        var totalAulasParaAluno = ObterTotalAulasAluno(registroFreqAlunos, codigoAluno);
                        excluirFrequenciaAlunoIds.AddRange(ObterIdsFrequenciaAlunoSemAulaDisciplinaExclusao(totalAulasNaDisciplinaParaAluno, frequenciaDosAlunos, codigoAluno, disciplinasIdsConsideradas));
                        excluirFrequenciaAlunoIds.AddRange(ObterIdsFrequenciaAlunoSemAulaExclusao(totalAulasParaAluno, frequenciaDosAlunos, codigoAluno));
                        
                        TrataFrequenciaPorDisciplinaAluno(codigoAluno, totalAulasNaDisciplinaParaAluno, registroFrequenciaAgregado, frequenciaDosAlunos, totalCompensacoesDisciplinaAlunos, request.DisciplinaId, excluirFrequenciaAlunoIds);
                        TrataFrequenciaGlobalAluno(codigoAluno, totalAulasParaAluno, registroFrequenciaAgregado, frequenciaDosAlunos, totalCompensacoesDisciplinaAlunos, request.TurmaId);
                    }

                    VerificaExclusaoFrequenciaConsolidadaMasNaoLancada(registroFreqAlunos, frequenciaDosAlunos, excluirFrequenciaAlunoIds);
                    await TrataPersistencia(frequenciaDosAlunos);
                }
                else if (frequenciaDosAlunos.Any())
                {
                    excluirFrequenciaAlunoIds.AddRange(frequenciaDosAlunos.Select(s => s.Id));
                }

                if (excluirFrequenciaAlunoIds.Any())
                    await ExcluirFrequenciaAluno(excluirFrequenciaAlunoIds);
            }
        }

        private IEnumerable<long> ObterIdsFrequenciaAlunoSemAulaDisciplinaExclusao(int totalAulasNaDisciplinaParaAluno, IEnumerable<FrequenciaAluno> frequenciaAlunos,
                                                     string codigoAluno, IEnumerable<string> disciplinasIdsConsideradas)
        {
            if (totalAulasNaDisciplinaParaAluno == 0)
                return frequenciaAlunos
                                .Where(w => w.Tipo == TipoFrequenciaAluno.PorDisciplina && disciplinasIdsConsideradas.Contains(w.DisciplinaId) && w.CodigoAluno.Equals(codigoAluno))
                                .Select(s => s.Id);
            return Enumerable.Empty<long>();
        }

        private IEnumerable<long> ObterIdsFrequenciaAlunoSemAulaExclusao(int totalAulasParaAluno, IEnumerable<FrequenciaAluno> frequenciaAlunos,
                                                     string codigoAluno)
        {
            if (totalAulasParaAluno == 0)
                return frequenciaAlunos
                                .Where(w => w.Tipo == TipoFrequenciaAluno.Geral && w.CodigoAluno.Equals(codigoAluno))
                                .Select(s => s.Id);
            return Enumerable.Empty<long>();
        }

        private int ObterTotalAulasAluno(IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registroFreqAlunos,
                                                     string codigoAluno)
        {
            return registroFreqAlunos
                            .Where(t => t.AlunoCodigo.Equals(codigoAluno))
                            .Sum(s => s.TotalAulas);
        }

        private int ObterTotalAulasAlunoNaDisciplina(IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registroFreqAlunos, 
                                                     string codigoAluno, IEnumerable<string> disciplinasIdsConsideradas)
        {
            return registroFreqAlunos
                            .Where(t => t.AlunoCodigo.Equals(codigoAluno) && disciplinasIdsConsideradas.Contains(t.ComponenteCurricularId))?
                            .Sum(t => t.TotalAulas) ?? 0;
        }
        private PeriodoEscolar ObterPeriodoVigenteDataAula(IEnumerable<PeriodoEscolar> periodos, DateTime dataAula)
        {
            var periodo = periodos?.SingleOrDefault(p => p.PeriodoInicio.Date <= dataAula && p.PeriodoFim.Date >= dataAula);
            if (periodo.EhNulo())
                throw new NegocioException("A data da aula está fora dos períodos escolares da turma");
            return periodo;
        }

        private async Task<string> ObterRfProfessorLogado(CalcularFrequenciaPorTurmaCommand request)
        {
            var usuarioConsiderado = await ObterUsuarioConsiderado(request);
            return usuarioConsiderado.NaoEhNulo() && usuarioConsiderado.EhProfessor() ? usuarioConsiderado.CodigoRf : null;
        }

        private async Task<Usuario> ObterUsuarioConsiderado(CalcularFrequenciaPorTurmaCommand request)
        {
            var usuario = request.UsuarioConsiderado != default ?
                await mediator.Send(new ObterUsuarioPorRfQuery(request.UsuarioConsiderado.rf)) : null;

            if (usuario != null)
                usuario.PerfilAtual = Guid.Parse(request.UsuarioConsiderado.perfil);

            return usuario;
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

        private void VerificaFrequenciasReplicadasIndevidamente(IEnumerable<FrequenciaAluno> frequenciasAlunos, string[] disciplinasId, List<long> frequenciaAlunosIdsParaExcluir, int bimestre)
        {
            var frequenciaAgrupada = frequenciasAlunos.Where(f => f.Tipo == TipoFrequenciaAluno.PorDisciplina)
                .GroupBy(f => (f.CodigoAluno, f.Bimestre, f.Tipo, f.Professor));

            foreach (var frequencia in frequenciaAgrupada)
            {
                var frequenciasDuplicadasPorDisciplina = frequencia
                    .Where(f => disciplinasId.Contains(f.DisciplinaId) && f.Bimestre == bimestre).Select(f => f.Id);

                if (frequenciasDuplicadasPorDisciplina.Count() > 1)
                {
                    frequenciaAlunosIdsParaExcluir
                        .AddRange(frequenciasDuplicadasPorDisciplina);

                    frequenciaAlunosIdsParaExcluir
                        .Remove(frequenciasDuplicadasPorDisciplina.OrderBy(f => f).Last()); // mantém somente uma frequência na lista para fazer a alteração na base com os dados atualizados
                }
            }

            var frequenciaAgrupadaGeral = frequenciasAlunos.Where(f => f.Tipo == TipoFrequenciaAluno.Geral)
                .GroupBy(f => (f.CodigoAluno, f.Bimestre, f.Tipo));

            foreach (var frequencia in frequenciaAgrupadaGeral)
            {
                var frequenciasDuplicadasGeral = frequencia
                    .Where(f => string.IsNullOrWhiteSpace(f.DisciplinaId) && f.Bimestre == bimestre).Select(f => f.Id);

                if (frequenciasDuplicadasGeral.Count() > 1)
                {
                    frequenciaAlunosIdsParaExcluir
                        .AddRange(frequenciasDuplicadasGeral);

                    frequenciaAlunosIdsParaExcluir
                        .Remove(frequenciasDuplicadasGeral.OrderBy(f => f).Last()); // mantém somente uma frequência na lista para fazer a alteração na base com os dados atualizados
                }
            }
        }

        private async Task ExcluirFrequenciaAluno(List<long> excluirFrequenciaAlunoIds)
        {
            if (excluirFrequenciaAlunoIds.Any())
                await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverVariosAsync(excluirFrequenciaAlunoIds.ToArray());
        }

        private List<RegistroFrequenciaPorDisciplinaAlunoDto> ObterRegistroFrequenciaAgregado(IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registroFreqAlunos)
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

        private async Task TrataPersistencia(IEnumerable<FrequenciaAluno> frequenciasParaPersistir)
        {
            await policy.ExecuteAsync(() => Persistir(frequenciasParaPersistir));
        }

        private async Task Persistir(IEnumerable<FrequenciaAluno> frequenciasParaPersistir)
        {
            if (frequenciasParaPersistir.NaoEhNulo() && frequenciasParaPersistir.Any())
            {
                if (frequenciasParaPersistir.Any(a => a.FrequenciaNegativa()))
                    throw new NegocioException($"Erro ao calcular frequencia da turma {frequenciasParaPersistir.First().TurmaId} : Número de ausências maior do que o número de aulas");

                foreach (var frequenciaAluno in frequenciasParaPersistir)
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
            }
        }

        private void TrataFrequenciaPorDisciplinaAluno(string alunoCodigo, int totalAulasNaDisciplina,
                                                       IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos,
                                                       List<FrequenciaAluno> frequenciaDosAlunos,
                                                       IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
                                                       string componenteCurricularId, 
                                                       List<long> excluirFrequenciaAlunoIds)
        {
            var registrosFrequenciaDisciplina = from rf in registroFrequenciaAlunos
                                                where rf.ComponenteCurricularId == componenteCurricularId 
                                                select rf;

            if (registrosFrequenciaDisciplina.Any() || totalAulasNaDisciplina > 0)
            {
                var registrosFrequenciaAluno = registrosFrequenciaDisciplina
                    .Where(a => a.AlunoCodigo == alunoCodigo);

                var frequenciasAluno = from f in frequenciaDosAlunos
                                       where f.CodigoAluno == alunoCodigo &&
                                             (f.DisciplinaId == componenteCurricularId) &&
                                             f.Tipo == TipoFrequenciaAluno.PorDisciplina
                                             && f.Bimestre == periodoConsiderado.Bimestre
                                       orderby f.Id
                                       select f;

                var frequenciaParaTratar = frequenciasAluno.LastOrDefault();
                var totalCompensacoes = 0;
                var totalCompensacoesDisciplinaAluno = (from c in compensacoesDisciplinasAlunos
                                                        where c.AlunoCodigo == alunoCodigo &&
                                                              (c.ComponenteCurricularId == componenteCurricularId)
                                                              && c.Bimestre == periodoConsiderado.Bimestre
                                                        select c).FirstOrDefault();

                if (totalCompensacoesDisciplinaAluno.NaoEhNulo())
                    totalCompensacoes = totalCompensacoesDisciplinaAluno.Compensacoes;

                var totalAusencias = registrosFrequenciaAluno?.Sum(rfa => rfa.TotalAusencias) ?? 0;

                if (registrosFrequenciaAluno.NaoEhNulo() && registrosFrequenciaAluno.Any())
                {
                    if (frequenciaParaTratar.EhNulo())
                    {
                        frequenciaParaTratar = new FrequenciaAluno(
                                     alunoCodigo,
                                     turma.CodigoTurma,
                                     componenteCurricularId,
                                     registrosFrequenciaAluno?.First().PeriodoEscolarId ?? periodoConsiderado.Id,
                                     registrosFrequenciaAluno?.First().PeriodoInicio ?? periodoConsiderado.PeriodoInicio,
                                     registrosFrequenciaAluno?.First().PeriodoFim ?? periodoConsiderado.PeriodoFim,
                                     registrosFrequenciaAluno?.First().Bimestre ?? periodoConsiderado.Bimestre,
                                     totalAusencias > totalAulasNaDisciplina ? totalAulasNaDisciplina : totalAusencias,
                                     totalAulasNaDisciplina,
                                     totalAusencias >= totalCompensacoes ? totalCompensacoes : totalAusencias,
                                     TipoFrequenciaAluno.PorDisciplina,
                                     registrosFrequenciaAluno?.Sum(rfa => rfa.TotalRemotos) ?? 0,
                                     registrosFrequenciaAluno?.Sum(rfa => rfa.TotalPresencas) ?? totalAulasNaDisciplina,
                                     null);

                        frequenciaDosAlunos.Add(frequenciaParaTratar);
                    }
                    else
                    {
                        var totalCompensacoesDisciplinas = totalCompensacoesDisciplinaAluno?.Compensacoes ?? 0;
                        frequenciaParaTratar
                            .DefinirFrequencia(componenteCurricularId,
                                               totalAusencias > totalAulasNaDisciplina ? totalAulasNaDisciplina : totalAusencias,
                                               totalAulasNaDisciplina,
                                               totalAusencias >= totalCompensacoes ? totalCompensacoes : totalAusencias,
                                               TipoFrequenciaAluno.PorDisciplina,
                                               registrosFrequenciaAluno?.Sum(rfa => rfa.TotalRemotos) ?? 0,
                                               registrosFrequenciaAluno?.Sum(rfa => rfa.TotalPresencas) ?? totalAulasNaDisciplina,
                                               null);
                    }

                    var frequenciasDoAlunoSemProfessor = frequenciasAluno
                        .Except(new List<FrequenciaAluno>() { frequenciaParaTratar })
                        .Where(f => string.IsNullOrEmpty(f.Professor));

                    if (frequenciasDoAlunoSemProfessor.Any())
                        excluirFrequenciaAlunoIds.AddRange(frequenciasDoAlunoSemProfessor.Select(f => f.Id));
                }
            }
        }

        private void TrataFrequenciaGlobalAluno(string alunoCodigo,
                                                int totalAulasDaTurmaGeral,
                                                IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos,
                                                List<FrequenciaAluno> frequenciaDosAlunos,
                                                IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
                                                string turmaId)
        {
            if (registroFrequenciaAlunos.Any() && totalAulasDaTurmaGeral > 0)
            {
                var registroFrequenciaAluno = ObterRegistroFrequenciaDisciplinaAluno(registroFrequenciaAlunos, alunoCodigo); 
                var totalCompensacoesAluno = compensacoesDisciplinasAlunos
                                                    .Where(a => a.AlunoCodigo == alunoCodigo)
                                                    .Sum(c => c.Compensacoes);

                var frequenciaParaTratar = ObterFrequenciaAluno(frequenciaDosAlunos, alunoCodigo, registroFrequenciaAluno.Bimestre);

                if (frequenciaParaTratar.EhNulo())
                {
                    var frequenciaGlobal = new FrequenciaAluno(
                                 alunoCodigo,
                                 turmaId,
                                 string.Empty,
                                 registroFrequenciaAluno.PeriodoEscolarId,
                                 registroFrequenciaAluno.PeriodoInicio,
                                 registroFrequenciaAluno.PeriodoFim,
                                 registroFrequenciaAluno.Bimestre,
                                 Math.Min(registroFrequenciaAluno.TotalAusencias, totalAulasDaTurmaGeral),
                                 totalAulasDaTurmaGeral,
                                 Math.Min(registroFrequenciaAluno.TotalAusencias, totalCompensacoesAluno),
                                 TipoFrequenciaAluno.Geral,
                                 registroFrequenciaAluno.TotalRemotos,
                                 registroFrequenciaAluno.TotalPresencas,
                                 null);

                    frequenciaDosAlunos.Add(frequenciaGlobal);
                }
                else
                {
                    frequenciaParaTratar
                        .DefinirFrequencia(string.Empty,
                                           Math.Min(registroFrequenciaAluno.TotalAusencias, totalAulasDaTurmaGeral),
                                           totalAulasDaTurmaGeral,
                                           Math.Min(registroFrequenciaAluno.TotalAusencias, totalCompensacoesAluno),
                                           TipoFrequenciaAluno.Geral,
                                           registroFrequenciaAluno.TotalRemotos,
                                           registroFrequenciaAluno.TotalPresencas,
                                           null);
                }
            }
        }

        private FrequenciaAluno ObterFrequenciaAluno(List<FrequenciaAluno> frequenciaDosAlunos, string alunoCodigo, int bimestre)
        {
            return (from f in frequenciaDosAlunos
                    where f.CodigoAluno == alunoCodigo &&
                          string.IsNullOrEmpty(f.DisciplinaId) &&
                          f.Bimestre == bimestre
                    orderby f.Id
                    select f)
                        .LastOrDefault();
        }

        private RegistroFrequenciaPorDisciplinaAlunoDto ObterRegistroFrequenciaDisciplinaAluno(IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos, string alunoCodigo)
        {
            return registroFrequenciaAlunos.Where(a => a.AlunoCodigo == alunoCodigo)
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
        }
    }
}