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
using static SME.SGP.Dominio.DateTimeExtension;

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

                var totalCompensacoesDisciplinaAlunos = await repositorioCompensacaoAusenciaAluno.ObterTotalCompensacoesPorAlunosETurmaAsync(bimestresParaFiltro.ToArray(), alunosComAusencias.ToArray(), request.TurmaId);

                foreach (var codigoAluno in alunosComAusencias)
                {
                    var ausenciasDoAluno = ausenciasDosAlunos.Where(a => a.AlunoCodigo == codigoAluno).ToList();

                    TrataFrequenciaAlunoComponente(request, frequenciaDosAlunos, frequenciasParaPersistir, totalAulasNaDisciplina, totalCompensacoesDisciplinaAlunos, codigoAluno, ausenciasDoAluno);
                    TrataFrequenciaAlunoGlobal(request, frequenciaDosAlunos, frequenciasParaPersistir, totalAulasDaTurmaGeral, totalCompensacoesDisciplinaAlunos, codigoAluno, ausenciasDoAluno);
                }
            }

            ObterFrequenciasParaExcluirGeral(request, frequenciaDosAlunos, frequenciasParaRemover, frequenciasParaPersistir);

            ObterFrequenciasParaRemoverAlunosSemAusencia(request, ausenciasDosAlunos, frequenciaDosAlunos, frequenciasParaRemover);

            await TrataPersistencia(frequenciasParaRemover, frequenciasParaPersistir);

            return true;
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

        private static void ObterFrequenciasParaRemoverAlunosSemAusencia(CalcularFrequenciaPorTurmaCommand request, IEnumerable<AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, List<FrequenciaAluno> frequenciasParaRemover)
        {
            var alunosSemAusencia = frequenciaDosAlunos.Where(f => f.Tipo == TipoFrequenciaAluno.PorDisciplina &&
                                                                   !ausenciasDosAlunos.Any(a => a.AlunoCodigo == f.CodigoAluno &&
                                                                                                a.ComponenteCurricularId == f.DisciplinaId &&
                                                                                                a.PeriodoEscolarId == f.PeriodoEscolarId)).ToList();

            if (alunosSemAusencia != null && alunosSemAusencia.Any())
                frequenciasParaRemover.AddRange(alunosSemAusencia);
        }

        private void TrataFrequenciaAlunoGlobal(CalcularFrequenciaPorTurmaCommand request, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, List<FrequenciaAluno> frequenciasParaPersistir, int totalAulasDaTurmaGeral, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> totalCompensacoesDisciplinaAlunos, string codigoAluno, List<AusenciaPorDisciplinaAlunoDto> ausenciasDoAluno)
        {
            var frequenciaGlobalAluno = TrataFrequenciaGlobalAluno(codigoAluno, totalAulasDaTurmaGeral, ausenciasDoAluno, frequenciaDosAlunos,
                                         totalCompensacoesDisciplinaAlunos, request.TurmaId);

            if (frequenciaGlobalAluno != null)
                frequenciasParaPersistir.Add(frequenciaGlobalAluno);
        }

        private void TrataFrequenciaAlunoComponente(CalcularFrequenciaPorTurmaCommand request, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, List<FrequenciaAluno> frequenciasParaPersistir, int totalAulasNaDisciplina, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> totalCompensacoesDisciplinaAlunos, string codigoAluno, List<AusenciaPorDisciplinaAlunoDto> ausenciasDoAluno)
        {
            var frequenciaDisciplinaAluno = TrataFrequenciaPorDisciplinaAluno(codigoAluno, totalAulasNaDisciplina, ausenciasDoAluno, frequenciaDosAlunos,
                totalCompensacoesDisciplinaAlunos, request.TurmaId, request.DisciplinaId);

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

            //if (idsFinaisParaRemover != null && idsFinaisParaRemover.Any())
            //{
            //    await repositorioFrequenciaAlunoDisciplinaPeriodo.RemoverVariosAsync(idsFinaisParaRemover);
            //}

            if (frequenciasParaPersistir != null && frequenciasParaPersistir.Any())
            {
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

        private FrequenciaAluno TrataFrequenciaPorDisciplinaAluno(string alunoCodigo, int totalAulasNaDisciplina, IEnumerable<Infra.AusenciaPorDisciplinaAlunoDto> ausenciasDosAlunos,
            IEnumerable<FrequenciaAluno> frequenciaDosAlunos, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
            string turmaId, string componenteCurricularId)
        {
            FrequenciaAluno frequenciaFinal;

            var ausenciasDoAlunoPorDisciplina = ausenciasDosAlunos.FirstOrDefault(a => a.ComponenteCurricularId == componenteCurricularId);

            if (ausenciasDoAlunoPorDisciplina == null || ausenciasDoAlunoPorDisciplina.TotalAusencias == 0)
            {
                return null;
            }
            else
            {
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
                                 ausenciasDoAlunoPorDisciplina.PeriodoEscolarId,
                                 ausenciasDoAlunoPorDisciplina.PeriodoInicio,
                                 ausenciasDoAlunoPorDisciplina.PeriodoFim,
                                 ausenciasDoAlunoPorDisciplina.Bimestre,
                                 ausenciasDoAlunoPorDisciplina.TotalAusencias,
                                 totalAulasNaDisciplina,
                                 totalCompensacoes,
                                 TipoFrequenciaAluno.PorDisciplina);
                }
                else
                {
                    frequenciaFinal = frequenciaParaTratar.DefinirFrequencia(ausenciasDoAlunoPorDisciplina.TotalAusencias, totalAulasNaDisciplina, (totalCompensacoesDisciplinaAluno?.Compensacoes ?? 0), TipoFrequenciaAluno.PorDisciplina);
                }
            }
            return frequenciaFinal;
        }

        private FrequenciaAluno TrataFrequenciaGlobalAluno(string alunoCodigo, int totalAulasDaTurmaGeral,
        IEnumerable<Infra.AusenciaPorDisciplinaAlunoDto> ausenciasDoAlunos, IEnumerable<FrequenciaAluno> frequenciaDosAlunos, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> compensacoesDisciplinasAlunos,
        string turmaId)
        {
            FrequenciaAluno frequenciaGlobal;

            if (ausenciasDoAlunos == null || !ausenciasDoAlunos.Any())
            {
                return null;
            }
            else
            {
                var ausenciaParaSeBasear = ausenciasDoAlunos.FirstOrDefault();

                int totalCompensacoesDoAlunoGeral = 0, totalAusencias = 0;

                totalAusencias = ausenciasDoAlunos.Sum(a => a.TotalAusencias);

                var totaisDoAluno = compensacoesDisciplinasAlunos.Where(a => a.AlunoCodigo == alunoCodigo).ToList();
                if (totaisDoAluno.Any())
                {
                    totalCompensacoesDoAlunoGeral = totaisDoAluno.Sum(a => a.Compensacoes);
                }

                var frequenciaParaTratar = frequenciaDosAlunos.FirstOrDefault(a => a.CodigoAluno == alunoCodigo && string.IsNullOrEmpty(a.DisciplinaId));
                if (frequenciaParaTratar == null)
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
            }
            return frequenciaGlobal;
        }

    }
}