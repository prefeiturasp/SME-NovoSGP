using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandHandler : IRequestHandler<RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;

        public RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno,
                                                                                IRepositorioCache repositorioCache,
                                                                                IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand request, CancellationToken cancellationToken)
        {
            var frequenciaAluno = await mediator
                .Send(new ObterFrequenciaAlunoPorIdQuery(request.FrequenciaAlunoId), cancellationToken);

            if (frequenciaAluno != null)
            {
                var datasConsideradas = new List<DateTime>() { frequenciaAluno.PeriodoFim };

                var alunoTurma = await mediator
                    .Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(frequenciaAluno.TurmaId), int.Parse(frequenciaAluno.CodigoAluno)), cancellationToken);

                if (alunoTurma != null)
                {
                    var registrosFrequenciaForaPeriodoMatriculaAluno =
                        await ObterRegistrosFrequenciaForaPeriodoMatriculaAluno(frequenciaAluno, alunoTurma, cancellationToken);

                    if (registrosFrequenciaForaPeriodoMatriculaAluno != null && registrosFrequenciaForaPeriodoMatriculaAluno.Any())
                    {
                        datasConsideradas.AddRange(registrosFrequenciaForaPeriodoMatriculaAluno.Select(rf => rf.DataAula));
                        await ExcluirRegistrosFrequenciaGravarCache(frequenciaAluno, registrosFrequenciaForaPeriodoMatriculaAluno);
                    }
                }

                await AcionarCalculoFrequencia(frequenciaAluno, datasConsideradas, cancellationToken);
            }

            return true;
        }

        private async Task AcionarCalculoFrequencia(FrequenciaAluno frequenciaAluno, List<DateTime> dataConsideradas, CancellationToken cancellationToken)
        {
            foreach (var dataRerefencia in dataConsideradas.Select(d => d.Date).Distinct())
            {
                var componenteParaCalculo = frequenciaAluno.Tipo == TipoFrequenciaAluno.Geral ?
                    await ObterDisciplinaParaCalculo(frequenciaAluno, cancellationToken) : (string)null;

                var commandCalculo =
                    new CalcularFrequenciaPorTurmaCommand(new string[] { frequenciaAluno.CodigoAluno }, dataRerefencia.Date, frequenciaAluno.TurmaId, componenteParaCalculo ?? frequenciaAluno.DisciplinaId);

                await mediator
                    .Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaCalculoFrequenciaPorTurmaComponente, commandCalculo), cancellationToken);
            }
        }

        private async Task<string> ObterDisciplinaParaCalculo(FrequenciaAluno frequenciaAluno, CancellationToken cancellationToken)
        {
            var frequenciaPorDisciplina = await mediator
                .Send(new ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery(frequenciaAluno.CodigoAluno, frequenciaAluno.Bimestre, TipoFrequenciaAluno.PorDisciplina, frequenciaAluno.TurmaId), cancellationToken);

            if (frequenciaPorDisciplina != null)
                return frequenciaPorDisciplina.DisciplinaId;

            var componentesTurma = await mediator
                .Send(new ObterDisciplinasPorCodigoTurmaQuery(frequenciaAluno.TurmaId), cancellationToken);

            return componentesTurma
                .First(cc => cc.RegistroFrequencia).CodigoComponenteCurricular.ToString();
        }

        private async Task ExcluirRegistrosFrequenciaGravarCache(FrequenciaAluno frequenciaAluno, IEnumerable<FrequenciaAlunoTurmaDto> registrosFrequenciaForaPeriodoMatriculaAluno)
        {
            var registrosExclusao = registrosFrequenciaForaPeriodoMatriculaAluno
                .Select(rf => rf.RegistroFrequenciaAlunoId);

            await repositorioRegistroFrequenciaAluno
                .ExcluirVarios(registrosExclusao.ToList());

            await GravarRegistrosExcluidosCache(frequenciaAluno, registrosExclusao);
        }

        private async Task GravarRegistrosExcluidosCache(FrequenciaAluno frequenciaAluno, IEnumerable<long> registrosExclusao)
        {
            var chaveCache = string.Format(NomeChaveCache.REGISTROS_FREQUENCIA_ALUNO_EXCLUIDOS_TURMA, frequenciaAluno.TurmaId);

            await repositorioCache
                .SalvarAsync(chaveCache, registrosExclusao.ToArray(), 300);
        }

        private async Task<IEnumerable<FrequenciaAlunoTurmaDto>> ObterRegistrosFrequenciaForaPeriodoMatriculaAluno(FrequenciaAluno frequenciaAluno, IEnumerable<AlunoPorTurmaResposta> alunoTurma, CancellationToken cancellationToken)
        {
            var registrosFrequenciaAluno = await mediator
                .Send(new ObterRegistroFrequenciaAlunoPorTurmaEAlunoCodigoQuery(frequenciaAluno.TurmaId, frequenciaAluno.CodigoAluno), cancellationToken);

            var registrosFrequenciaForaPeriodoMatriculaAluno = from rf in registrosFrequenciaAluno
                                                               from at in alunoTurma
                                                               where (at.CodigoSituacaoMatricula == SituacaoMatriculaAluno.VinculoIndevido) ||
                                                                     (at.Ativo && rf.DataAula.Date <= at.DataMatricula.Date) ||                                                                      
                                                                     (at.Inativo && (rf.DataAula.Date <= at.DataMatricula.Date || rf.DataAula.Date > at.DataSituacao.Date))
                                                               select rf;

            return registrosFrequenciaForaPeriodoMatriculaAluno;
        }
    }
}
