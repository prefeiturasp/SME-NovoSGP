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

            var ausenciasDosAlunos = await repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunosETurmaAsync(request.DataAula, request.Alunos, request.TurmaId);

            var periodosEscolaresParaFiltro = ausenciasDosAlunos.Select(a => a.PeriodoEscolarId).Distinct().ToList();

            var frequenciaDosAlunos = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunosAsync(request.Alunos, periodosEscolaresParaFiltro, request.TurmaId);

            var frequenciasParaRemover = new List<FrequenciaAluno>();
            var frequenciasParaPersistir = new List<FrequenciaAluno>();

            if (ausenciasDosAlunos != null && ausenciasDosAlunos.Any())
            {
                //Transformar em uma query única?
                var totalAulasNaDisciplina = await repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, request.DisciplinaId, request.TurmaId);
                var totalAulasDaTurmaGeral = await repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurmaAsync(request.DataAula, string.Empty, request.TurmaId);
                //

                var alunosComAusencias = ausenciasDosAlunos.Select(a => a.AlunoCodigo).Distinct().ToList();
                var bimestresParaFiltro = ausenciasDosAlunos.Select(a => a.Bimestre).Distinct().ToList();

                var totalCompensacoesDisciplinaAlunos = await repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunosETurmaAsync(bimestresParaFiltro, alunosComAusencias, request.TurmaId);

                foreach (var codigoAluno in request.Alunos)
                {
                    var ausenciasDoAluno = ausenciasDosAlunos.Where(a => a.AlunoCodigo == codigoAluno).ToList();
                    var compensacoesDoAluno = totalCompensacoesDisciplinaAlunos.Where(a => a.AlunoCodigo == codigoAluno).ToList();
                    var frequenciasDoAluno = frequenciaDosAlunos.Where( a=> a.CodigoAluno == codigoAluno).ToList();

                    TrataFrequenciaPorDisciplinaAluno(
                        codigoAluno,
                        totalAulasNaDisciplina,
                        ausenciasDoAluno,
                        frequenciaDosAlunos,
                        totalCompensacoesDisciplinaAlunos,
                        request.TurmaId,
                        request.DisciplinaId,
                        frequenciasParaPersistir,
                        frequenciasParaRemover);

                    TrataFrequenciaGlobalAluno(
                        codigoAluno,
                        totalAulasDaTurmaGeral,
                        ausenciasDoAluno,
                        frequenciaDosAlunos,
                        totalCompensacoesDisciplinaAlunos,
                        request.TurmaId,
                        frequenciasParaPersistir,
                        frequenciasParaRemover);
                }
            }

            ObterFrequenciasParaExcluirGeral(frequenciaDosAlunos, frequenciasParaRemover, ausenciasDosAlunos);

            ObterFrequenciasParaRemoverAlunosSemAusencia(ausenciasDosAlunos, frequenciaDosAlunos, frequenciasParaRemover);

            await TrataPersistencia(frequenciasParaRemover, frequenciasParaPersistir);

            return true;
        }

        private static void ObterFrequenciasParaExcluirGeral(IEnumerable<FrequenciaAluno> frequenciaDosAlunos, List<FrequenciaAluno> frequenciasParaRemover, IEnumerable<AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos)
        {
            var alunosSemAusencia = frequenciaDosAlunos.Where(f => f.Tipo == TipoFrequenciaAluno.Geral &&
                                                                             !ausenciasDosAlunos.Any(a => a.AlunoCodigo == f.CodigoAluno &&
                                                                                               a.PeriodoEscolarId == f.PeriodoEscolarId)).ToList();

            frequenciasParaRemover.AddRange(alunosSemAusencia);
        }

        private static void ObterFrequenciasParaRemoverAlunosSemAusencia(IEnumerable<AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, 
            List<FrequenciaAluno> frequenciasParaRemover)
        {
            var alunosSemAusencia = frequenciaDosAlunos.Where(f => f.Tipo == TipoFrequenciaAluno.PorDisciplina &&
                                                                   !ausenciasDosAlunos.Any(a => a.AlunoCodigo == f.CodigoAluno &&
                                                                                                a.ComponenteCurricularId == f.DisciplinaId &&
                                                                                                a.PeriodoEscolarId == f.PeriodoEscolarId)).ToList();

            if (alunosSemAusencia != null && alunosSemAusencia.Any())
                frequenciasParaRemover.AddRange(alunosSemAusencia);
        }

        private async Task TrataPersistencia(List<FrequenciaAluno> frequenciasParaRemover, List<FrequenciaAluno> frequenciasParaPersistir)
        {
            var idsParaRemover = new List<long>();

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
                var alunos = frequenciasParaPersistir.Select(a => a.CodigoAluno).Distinct().ToArray();
                var frequencia = frequenciasParaPersistir.FirstOrDefault();
                var periodoEscolarId = frequencia.PeriodoEscolarId.Value;
                var turmaCodigo = frequencia.TurmaId;

                unitOfWork.IniciarTransacao();
                try
                {
                    //await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverFrequenciaGeralAlunos(alunos, turmaCodigo, periodoEscolarId);

                    foreach (var frequenciaAluno in frequenciasParaPersistir)
                    {
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

        private void TrataFrequenciaPorDisciplinaAluno(string alunoCodigo, int totalAulasNaDisciplina, IEnumerable<Infra.AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos,
            IEnumerable<FrequenciaAluno> frequenciaDosAlunos, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
            string turmaId, string componenteCurricularId, List<FrequenciaAluno> frequenciasParaPersistir, List<FrequenciaAluno> frequenciasParaRemover)
        {
            var ausenciasDoAlunoPorDisciplina = ausenciasDosAlunos.FirstOrDefault(a => a.ComponenteCurricularId == componenteCurricularId);

            var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => a.DisciplinaId == componenteCurricularId);

            if (frequenciaParaTratar != null && (ausenciasDoAlunoPorDisciplina == null || ausenciasDoAlunoPorDisciplina.TotalAusencias == 0))
            {
                frequenciasParaRemover.Add(frequenciaParaTratar);
                return;
            }

            var totalCompensacoes = 0;

            var totalCompensacoesDisciplinaAluno = compensacoesDisciplinasAlunos.FirstOrDefault(a => a.ComponenteCurricularId == componenteCurricularId);
            if (totalCompensacoesDisciplinaAluno != null)
                totalCompensacoes = totalCompensacoesDisciplinaAluno.Compensacoes;

            if (ausenciasDoAlunoPorDisciplina != null)
            {
                var frequenciaFinal = frequenciaParaTratar == null ?
                    new FrequenciaAluno(
                            alunoCodigo,
                            turmaId,
                            componenteCurricularId,
                            ausenciasDoAlunoPorDisciplina.PeriodoEscolarId,
                            ausenciasDoAlunoPorDisciplina.PeriodoInicio,
                            ausenciasDoAlunoPorDisciplina.PeriodoFim,
                            ausenciasDoAlunoPorDisciplina.Bimestre,
                            ausenciasDoAlunoPorDisciplina.TotalAusencias,
                            totalAulasNaDisciplina,
                            totalCompensacoes,
                            TipoFrequenciaAluno.PorDisciplina) :
                    frequenciaParaTratar.DefinirFrequencia(
                        ausenciasDoAlunoPorDisciplina.TotalAusencias,
                        totalAulasNaDisciplina,
                        (totalCompensacoesDisciplinaAluno?.Compensacoes ?? 0),
                        TipoFrequenciaAluno.PorDisciplina);

                frequenciasParaPersistir.Add(frequenciaFinal);
            }
        }

        private void TrataFrequenciaGlobalAluno(string alunoCodigo, int totalAulasDaTurmaGeral,
        IEnumerable<Infra.AusenciaPorDisciplinaAlunoDto> ausenciasDoAlunos, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
        string turmaId, List<FrequenciaAluno> frequenciasParaPersistir, List<FrequenciaAluno> frequenciasParaRemover)
        {
            FrequenciaAluno frequenciaGlobal;

            //TODO: Caso não tenha ausência? Avaliar

            var ausenciaParaSeBasear = ausenciasDoAlunos.FirstOrDefault();

            int totalCompensacoesDoAlunoGeral = 0, totalAusencias = 0;

            if (ausenciaParaSeBasear != null)
                totalAusencias = ausenciasDoAlunos.Sum(a => a.TotalAusencias);

            var totaisDoAluno = compensacoesDisciplinasAlunos.Where(a => a.AlunoCodigo == alunoCodigo).ToList();
            if (totaisDoAluno.Any())
            {
                totalCompensacoesDoAlunoGeral = totaisDoAluno.Sum(a => a.Compensacoes);
            }

            var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => string.IsNullOrEmpty(a.DisciplinaId));
            if (frequenciaParaTratar == null && ausenciaParaSeBasear != null)
            {
                frequenciaGlobal = new FrequenciaAluno
                         (
                             alunoCodigo,
                             turmaId,
                             string.Empty,
                             ausenciaParaSeBasear.PeriodoEscolarId,
                             ausenciaParaSeBasear.PeriodoInicio,
                             ausenciaParaSeBasear.PeriodoFim,
                             ausenciaParaSeBasear.Bimestre,
                             totalAusencias,
                             totalAulasDaTurmaGeral,
                             totalCompensacoesDoAlunoGeral,
                             TipoFrequenciaAluno.Geral);
            }
            else
            {
                frequenciaGlobal = frequenciaParaTratar.DefinirFrequencia(totalAusencias, totalAulasDaTurmaGeral, totalCompensacoesDoAlunoGeral, TipoFrequenciaAluno.Geral);
            }

            frequenciasParaPersistir.Add(frequenciaGlobal);
        }

    }
}