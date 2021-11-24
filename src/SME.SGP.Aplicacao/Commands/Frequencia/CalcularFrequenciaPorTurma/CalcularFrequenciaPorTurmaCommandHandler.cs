using MediatR;
using Polly;
using Polly.Registry;
using SME.GoogleClassroom.Infra;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<CalcularFrequenciaPorTurmaCommand, bool>
    {
        public readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        public readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IAsyncPolicy policy;

        public CalcularFrequenciaPorTurmaCommandHandler(IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo, IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
            IUnitOfWork unitOfWork, IMediator mediator, IReadOnlyPolicyRegistry<string> registry)
        {
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task<bool> Handle(CalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {

            if (request.Alunos == null || !request.Alunos.Any())
            {
                var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaId));
                if (alunosDaTurma == null || !alunosDaTurma.Any())
                    throw new NegocioException($"Não localizados alunos para turma [{request.TurmaId}] no EOL");

                request.Alunos = alunosDaTurma.Select(a => a.CodigoAluno).Distinct().ToList();
            }

            var registroFreqAlunos = (await mediator.Send(new ObterRegistroFrequenciaAlunosPorAlunosETurmaIdQuery(request.DataAula, request.Alunos, request.TurmaId))).ToList();

            var periodosEscolaresParaFiltro = registroFreqAlunos.Select(a => a.PeriodoEscolarId).Distinct().ToList();

            var frequenciaDosAlunos = (await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunosAsync(request.Alunos, periodosEscolaresParaFiltro, request.TurmaId)).ToList();

            var frequenciasParaRemover = new List<FrequenciaAluno>();
            var frequenciasParaPersistir = new List<FrequenciaAluno>();

            if (registroFreqAlunos.Any())
            {
                var aulasNaDisciplina = (await repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, request.DisciplinaId, request.TurmaId)).ToList();
                var totalAulasDaDisciplina = aulasNaDisciplina.Where(w => w.DisciplinaId == long.Parse(request.DisciplinaId)).Sum(s => s.Total);
                var totalAulasDaTurmaGeral = aulasNaDisciplina.Sum(s => s.Total);

                var alunosComFrequencia = registroFreqAlunos.Select(a => a.AlunoCodigo).Distinct().ToList();
                var bimestresParaFiltro = registroFreqAlunos.Select(a => a.Bimestre).Distinct().ToList();
                var registroFrequenciaAgregado = ObterRegistroFrequenciaAgregado(registroFreqAlunos);

                var totalCompensacoesDisciplinaAlunos = await repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunosETurmaAsync(bimestresParaFiltro, alunosComFrequencia, request.TurmaId);

                foreach (var codigoAluno in alunosComFrequencia)
                {
                    var ausenciasDoAluno = alunosComFrequencia.Where(a => a == codigoAluno).ToList();

                    TrataFrequenciaAlunoComponente(request, frequenciaDosAlunos, frequenciasParaPersistir, totalAulasDaDisciplina, totalCompensacoesDisciplinaAlunos, codigoAluno, registroFrequenciaAgregado);
                    TrataFrequenciaAlunoGlobal(request, frequenciaDosAlunos, frequenciasParaPersistir, totalAulasDaTurmaGeral, totalCompensacoesDisciplinaAlunos, codigoAluno, registroFrequenciaAgregado);
                }
            }

            ObterFrequenciasParaExcluirGeral(request, frequenciaDosAlunos, frequenciasParaRemover, frequenciasParaPersistir);

            ObterFrequenciasParaRemoverAlunosSemAusencia(request, registroFreqAlunos, frequenciaDosAlunos, frequenciasParaRemover);

            await TrataPersistencia(frequenciasParaRemover, frequenciasParaPersistir);

            return true;

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

        private static void ObterFrequenciasParaExcluirGeral(CalcularFrequenciaPorTurmaCommand request, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, List<FrequenciaAluno> frequenciasParaRemover, List<FrequenciaAluno> frequenciasParaPersistir)
        {
            var codigoAlunosParaPersistir = frequenciasParaPersistir.Select(a => a.CodigoAluno).Distinct().ToList();

            var frequenciasDaDisciplinaParaRemover = frequenciaDosAlunos.Where(a => a.DisciplinaId == request.DisciplinaId
                                                                                    && a.Tipo == TipoFrequenciaAluno.PorDisciplina
                                                                                    && codigoAlunosParaPersistir.Contains(a.CodigoAluno)).ToList();

            var frequenciasGlobaisParaRemover = frequenciaDosAlunos.Where(a => a.Tipo == TipoFrequenciaAluno.Geral
                                                                               && codigoAlunosParaPersistir.Contains(a.CodigoAluno)).ToList();

            frequenciasParaRemover.AddRange(frequenciasDaDisciplinaParaRemover);
            frequenciasParaRemover.AddRange(frequenciasGlobaisParaRemover);
        }

        private static void ObterFrequenciasParaRemoverAlunosSemAusencia(CalcularFrequenciaPorTurmaCommand request, List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, List<FrequenciaAluno> frequenciasParaRemover)
        {
            var alunosSemAusencia = frequenciaDosAlunos.Where(f => f.Tipo == TipoFrequenciaAluno.PorDisciplina &&
                                                                   !registroFrequenciaAlunos.Any(a => a.AlunoCodigo == f.CodigoAluno &&
                                                                                                a.ComponenteCurricularId == f.DisciplinaId &&
                                                                                                a.PeriodoEscolarId == f.PeriodoEscolarId &&
                                                                                                a.TipoFrequencia == (int)TipoFrequencia.F
                                                                                                )).ToList();

            if (alunosSemAusencia != null && alunosSemAusencia.Any())
                frequenciasParaRemover.AddRange(alunosSemAusencia);
        }

        private void TrataFrequenciaAlunoGlobal(CalcularFrequenciaPorTurmaCommand request,
                                                IEnumerable<FrequenciaAluno> frequenciaDosAlunos,
                                                List<FrequenciaAluno> frequenciasParaPersistir,
                                                int totalAulasDaTurmaGeral,
                                                IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> totalCompensacoesDisciplinaAlunos,
                                                string codigoAluno,
                                                List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos)
        {
            var frequenciaGlobalAluno = TrataFrequenciaGlobalAluno(codigoAluno,
                                                                   totalAulasDaTurmaGeral,
                                                                   registroFrequenciaAlunos,
                                                                   frequenciaDosAlunos,
                                                                   totalCompensacoesDisciplinaAlunos,
                                                                   request.TurmaId);

            if (frequenciaGlobalAluno != null)
                frequenciasParaPersistir.Add(frequenciaGlobalAluno);
        }

        private void TrataFrequenciaAlunoComponente(CalcularFrequenciaPorTurmaCommand request,
                                                    IEnumerable<FrequenciaAluno> frequenciaDosAlunos,
                                                    List<FrequenciaAluno> frequenciasParaPersistir,
                                                    int totalAulasNaDisciplina,
                                                    IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> totalCompensacoesDisciplinaAlunos,
                                                    string codigoAluno,
                                                    List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAluno)
        {
            var frequenciaDisciplinaAluno = TrataFrequenciaPorDisciplinaAluno(codigoAluno,
                                                                              totalAulasNaDisciplina,
                                                                              registroFrequenciaAluno,
                                                                              frequenciaDosAlunos,
                                                                              totalCompensacoesDisciplinaAlunos,
                                                                              request.TurmaId,
                                                                              request.DisciplinaId);

            if (frequenciaDisciplinaAluno != null)
                frequenciasParaPersistir.Add(frequenciaDisciplinaAluno);
        }

        private async Task TrataPersistencia(List<FrequenciaAluno> frequenciasParaRemover, List<FrequenciaAluno> frequenciasParaPersistir)
        {
            var idsParaRemover = new List<long>();

            if (frequenciasParaPersistir.Any())
            {
                idsParaRemover.AddRange(frequenciasParaPersistir
                  .Where(a => a.Id != 0)
                  .Select(a => a.Id)
                  .ToList());
            }

            if (frequenciasParaRemover.Any())
            {
                idsParaRemover.AddRange(frequenciasParaRemover
                 .Where(a => a.Id != 0)
                 .Select(a => a.Id)
                 .ToList());
            }

            var idsFinaisParaRemover = idsParaRemover.Distinct().ToArray();

            await policy.ExecuteAsync(() => Persistir(idsFinaisParaRemover, frequenciasParaPersistir));
        }

        private async Task Persistir(long[] idsFinaisParaRemover, List<FrequenciaAluno> frequenciasParaPersistir)
        {

            if (idsFinaisParaRemover != null && idsFinaisParaRemover.Any())
            {
                await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverVariosAsync(idsFinaisParaRemover);
            }

            if (frequenciasParaPersistir != null && frequenciasParaPersistir.Any())
            {
                if (frequenciasParaPersistir.Any(a => a.FrequenciaNegativa()))
                    throw new Exception($"Erro ao calcular frequencia da turma {frequenciasParaPersistir.First().TurmaId} : Número de ausências maior do que o número de aulas");

                var alunos = frequenciasParaPersistir.Select(a => a.CodigoAluno).Distinct().ToArray();
                var frequencia = frequenciasParaPersistir.FirstOrDefault();
                var periodoEscolarId = frequencia.PeriodoEscolarId.Value;
                var turmaCodigo = frequencia.TurmaId;

                unitOfWork.IniciarTransacao();
                try
                {
                    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverFrequenciaGeralAlunos(alunos, turmaCodigo, periodoEscolarId);

                    foreach (var frequenciaAluno in frequenciasParaPersistir)
                    {
                        frequenciaAluno.Id = 0;
                        await repositorioFrequenciaAlunoDisciplinaPeriodo.SalvarAsync(frequenciaAluno);
                    }

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }

                await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverFrequenciasDuplicadas(alunos, turmaCodigo, periodoEscolarId);
            }
        }

        private FrequenciaAluno TrataFrequenciaPorDisciplinaAluno(string alunoCodigo, int totalAulasNaDisciplina, List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos,
            IEnumerable<FrequenciaAluno> frequenciaDosAlunos, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
            string turmaId, string componenteCurricularId)
        {
            if (registroFrequenciaAlunos.Any(a => a.ComponenteCurricularId == componenteCurricularId))
            {
                FrequenciaAluno frequenciaFinal;

                var registroFrequenciaAluno = registroFrequenciaAlunos.FirstOrDefault(a => a.AlunoCodigo == alunoCodigo && a.ComponenteCurricularId == componenteCurricularId);

                var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => a.CodigoAluno == alunoCodigo && a.DisciplinaId == componenteCurricularId);
                var totalCompensacoes = 0;

                var totalCompensacoesDisciplinaAluno = compensacoesDisciplinasAlunos.FirstOrDefault(a => a.AlunoCodigo == alunoCodigo && a.ComponenteCurricularId == componenteCurricularId);
                if (totalCompensacoesDisciplinaAluno != null)
                    totalCompensacoes = totalCompensacoesDisciplinaAluno.Compensacoes;

                if (frequenciaParaTratar == null)
                {
                    frequenciaFinal = new FrequenciaAluno
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
                }
                else
                {
                    frequenciaFinal = frequenciaParaTratar.DefinirFrequencia(registroFrequenciaAluno.TotalAusencias, totalAulasNaDisciplina, (totalCompensacoesDisciplinaAluno?.Compensacoes ?? 0), TipoFrequenciaAluno.PorDisciplina, registroFrequenciaAluno.TotalRemotos, registroFrequenciaAluno.TotalPresencas);
                }
                return frequenciaFinal;
            }
            else
                return null;
        }

        private FrequenciaAluno TrataFrequenciaGlobalAluno(string alunoCodigo,
                                                           int totalAulasDaTurmaGeral,
                                                           List<RegistroFrequenciaPorDisciplinaAlunoDto> registroFrequenciaAlunos,
                                                           IEnumerable<FrequenciaAluno> frequenciaDosAlunos,
                                                           IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
                                                           string turmaId)
        {
            if (registroFrequenciaAlunos.Any())
            {
                FrequenciaAluno frequenciaGlobal;
                int totalCompensacoesDoAlunoGeral = 0;

                var registroFrequenciaAluno = registroFrequenciaAlunos.FirstOrDefault(a => a.AlunoCodigo == alunoCodigo);

                var totaisDoAluno = compensacoesDisciplinasAlunos.Where(a => a.AlunoCodigo == alunoCodigo).ToList();
                if (totaisDoAluno.Any())
                    totalCompensacoesDoAlunoGeral = totaisDoAluno.Sum(a => a.Compensacoes);

                var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => a.CodigoAluno == alunoCodigo && string.IsNullOrEmpty(a.DisciplinaId));
                if (frequenciaParaTratar == null)
                {
                    frequenciaGlobal = new FrequenciaAluno
                             (
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
                }
                else
                    frequenciaGlobal = frequenciaParaTratar.DefinirFrequencia(registroFrequenciaAluno.TotalAusencias, totalAulasDaTurmaGeral, totalCompensacoesDoAlunoGeral, TipoFrequenciaAluno.Geral, registroFrequenciaAluno.TotalRemotos, registroFrequenciaAluno.TotalPresencas);

                return frequenciaGlobal;
            }
            else
                return null;
        }

    }
}