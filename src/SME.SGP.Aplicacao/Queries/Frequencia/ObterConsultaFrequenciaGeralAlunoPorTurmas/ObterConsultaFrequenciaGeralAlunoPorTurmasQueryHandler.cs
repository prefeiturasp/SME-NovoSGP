using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandler : IRequestHandler<ObterConsultaFrequenciaGeralAlunoPorTurmasQuery, string>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        public ObterConsultaFrequenciaGeralAlunoPorTurmasQueryHandler(
                                            IMediator mediator, 
                                            IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<string> Handle(ObterConsultaFrequenciaGeralAlunoPorTurmasQuery request, CancellationToken cancellationToken)
        {
            var turma = request.TurmaConsulta ??
                await ObterTurma(request.TurmaCodigo);

            //Particularidade de 2020
            if (turma.AnoLetivo.Equals(2020))
                return await CalculoFrequenciaGlobal2020(request.AlunoCodigo, turma);

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            var alunosEol = await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma, consideraInativos: true));

            var dataMatriculaAluno = alunosEol.FirstOrDefault(a => a.CodigoAluno == request.AlunoCodigo)?.DataMatricula;
            return await mediator.Send(new ObterFrequenciaGeralAlunoPorTurmasQuery(request.AlunoCodigo, request.TurmaCodigo, tipoCalendarioId, dataMatriculaAluno));
        }

        private async Task<Turma> ObterTurma(string[] turmasCodigos)
        {
            var turmaCodigo = turmasCodigos.First();
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException("Turma não localizada.");

            return turma;
        }

        private async Task<string> CalculoFrequenciaGlobal2020(string alunoCodigo, Turma turma)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            var periodos = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
            var disciplinasDaTurmaEol = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(turma.CodigoTurma));
            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));
            var gruposMatrizes = disciplinasDaTurma.Where(c => c.RegistraFrequencia && c.GrupoMatrizNome != null).GroupBy(c => c.GrupoMatrizNome).ToList();
            var somaFrequenciaFinal = 0.0;
            var totalDisciplinas = 0;

            foreach (var grupoDisciplinasMatriz in gruposMatrizes.OrderBy(k => k.Key))
            {
                foreach (var disciplina in grupoDisciplinasMatriz)
                {
                    var somaPercentualFrequenciaDisciplinaBimestre = 0.0;
                    periodos.ToList().ForEach(p =>
                    {
                        var frequenciaAlunoPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo
                            .ObterPorAlunoBimestreAsync(alunoCodigo, p.Bimestre, TipoFrequenciaAluno.PorDisciplina, turma.CodigoTurma, disciplina.CodigoComponenteCurricular.ToString()).Result;

                        somaPercentualFrequenciaDisciplinaBimestre += frequenciaAlunoPeriodo?.PercentualFrequencia ?? 100;
                    });
                    var mediaFinalFrequenciaDiscipina = Math.Round(somaPercentualFrequenciaDisciplinaBimestre / periodos.Count(), 2);
                    somaFrequenciaFinal += mediaFinalFrequenciaDiscipina;
                }
                totalDisciplinas += grupoDisciplinasMatriz.Count();
            }

            var frequenciaGlobal2020 = Math.Round(somaFrequenciaFinal / totalDisciplinas, 2);

            if (frequenciaGlobal2020 == 0)
                return string.Empty;
            else
                return frequenciaGlobal2020.ToString();
        }
    }
}
