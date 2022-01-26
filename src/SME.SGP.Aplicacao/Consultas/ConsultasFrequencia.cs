using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFrequencia : IConsultasFrequencia
    {
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IServicoAluno servicoAluno;
        private readonly IObterInformacoesDeFrequenciaAlunoPorSemestreUseCase obterInformacoesDeFrequenciaAlunoPorSemestreUseCase;
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        private double _mediaFrequencia;

        public ConsultasFrequencia(IMediator mediator,
                                   IServicoEol servicoEOL,
                                   IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                    IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                   IConsultasTipoCalendario consultasTipoCalendario,
                                   IConsultasTurma consultasTurma,
                                   IRepositorioAula repositorioAula,
                                   IRepositorioFrequenciaConsulta repositorioFrequencia,
                                   IRepositorioTurma repositorioTurma,
                                   IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo,
                                   IRepositorioParametrosSistema repositorioParametrosSistema,
                                   IServicoAluno servicoAluno,
                                   IObterInformacoesDeFrequenciaAlunoPorSemestreUseCase obterInformacoesDeFrequenciaAlunoPorSemestreUseCase)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
            this.obterInformacoesDeFrequenciaAlunoPorSemestreUseCase = obterInformacoesDeFrequenciaAlunoPorSemestreUseCase ?? throw new ArgumentNullException(nameof(obterInformacoesDeFrequenciaAlunoPorSemestreUseCase));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<bool> FrequenciaAulaRegistrada(long aulaId)
            => await repositorioFrequencia.FrequenciaAulaRegistrada(aulaId);

        public async Task<string> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "", int? semestre = null)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));

            string[] turmasCodigos;

            if (turma.DeveVerificarRegraRegulares())
            {
                List<TipoTurma> turmasCodigosParaConsulta = new List<TipoTurma>() { turma.TipoTurma };
                turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, turmasCodigosParaConsulta, false));
            }
            else
                turmasCodigos = new string[1] { turma.CodigoTurma };

            if (!turmasCodigos.Any())
                turmasCodigos = new string[] { turma.CodigoTurma };

            return await ObterFrequenciaGeralAlunoPorTurmas(alunoCodigo, turmasCodigos, componenteCurricularCodigo, turma);
        }

        public async Task<FrequenciaAluno> ObterFrequenciaGeralAlunoPorTurmaEComponente(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "")
        {
            var frequenciaAlunoPeriodos = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralAluno(alunoCodigo, turmaCodigo, componenteCurricularCodigo);

            if (frequenciaAlunoPeriodos == null || !frequenciaAlunoPeriodos.Any())
                return null;

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciaAlunoPeriodos.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciaAlunoPeriodos.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciaAlunoPeriodos.Sum(f => f.TotalCompensacoes),
            };

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            var periodos = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);

            periodos.ToList().ForEach(p =>
            {
                var frequenciaCorrespondente = frequenciaAlunoPeriodos.SingleOrDefault(f => f.Bimestre == p.Bimestre);
                frequenciaAluno.AdicionarFrequenciaBimestre(p.Bimestre, frequenciaCorrespondente != null ? frequenciaCorrespondente.PercentualFrequencia : 100);
            });

            return frequenciaAluno;
        }

        public async Task<double> ObterFrequenciaMedia(DisciplinaDto disciplina, int anoLetivo)
        {
            if (_mediaFrequencia == 0)
            {
                if (disciplina.Regencia || !disciplina.LancaNota)
                    _mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse, anoLetivo)));
                else
                    _mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualFund2, anoLetivo)));
            }

            return _mediaFrequencia;
        }

        public async Task<IEnumerable<AlunoAusenteDto>> ObterListaAlunosComAusencia(string turmaId, string disciplinaId, int bimestre)
        {
            var alunosAusentesDto = new List<AlunoAusenteDto>();
            // Busca dados da turma
            var turma = await BuscaTurma(turmaId);

            // Busca periodo
            var periodo = await BuscaPeriodo(turma, bimestre);

            var alunosEOL = await mediator.Send(new ObterAlunosPorTurmaEDataMatriculaQuery(turmaId, periodo.PeriodoFim));

            var disciplinasEOL = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { long.Parse(disciplinaId) });
            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Componente curricular informado não localizado no EOL.");

            var quantidadeMaximaCompensacoes = int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia, DateTime.Today.Year)));
            var percentualFrequenciaAlerta = int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(disciplinasEOL.First().Regencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2, DateTime.Today.Year)));

            var alunosAtivos = alunosEOL.Where(a => a.CodigoSituacaoMatricula != SituacaoMatriculaAluno.RemanejadoSaida);
            foreach (var alunoEOL in alunosAtivos)
            {
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaData(alunoEOL.CodigoAluno, disciplinaId, periodo.PeriodoFim, turma.CodigoTurma);
                if (frequenciaAluno == null || frequenciaAluno.NumeroFaltasNaoCompensadas <= 0 || frequenciaAluno.PercentualFrequencia == 100)
                    continue;

                var faltasNaoCompensadas = int.Parse(frequenciaAluno.NumeroFaltasNaoCompensadas.ToString());

                alunosAusentesDto.Add(new AlunoAusenteDto()
                {
                    Id = alunoEOL.CodigoAluno,
                    Nome = alunoEOL.NomeAluno,
                    QuantidadeFaltasTotais = faltasNaoCompensadas,
                    MaximoCompensacoesPermitidas = quantidadeMaximaCompensacoes > faltasNaoCompensadas ? faltasNaoCompensadas : quantidadeMaximaCompensacoes,
                    PercentualFrequencia = frequenciaAluno.PercentualFrequencia,
                    Alerta = frequenciaAluno.PercentualFrequencia <= percentualFrequenciaAlerta
                });
            }

            return alunosAusentesDto;
        }

        public FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual, string turmaId)
            => repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaData(codigoAluno, disciplinaId, dataAtual, turmaId);

        public async Task<SinteseDto> ObterSinteseAluno(double? percentualFrequencia, DisciplinaDto disciplina, int anoLetivo)
        {
            var sintese = percentualFrequencia == null ?
                SinteseEnum.NaoFrequente :
                percentualFrequencia >= await ObterFrequenciaMedia(disciplina, anoLetivo) ?
                SinteseEnum.Frequente :
                SinteseEnum.NaoFrequente;

            return new SinteseDto()
            {
                Id = sintese,
                Valor = sintese.Name()
            };
        }

        private async Task<PeriodoEscolar> BuscaPeriodo(Turma turma, int bimestre)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi possível localizar o tipo de calendário da turma");

            var periodosEscolares = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi possível localizar os períodos escolares da turma");

            var periodoEscolar = periodosEscolares?.FirstOrDefault(p => p.Bimestre == bimestre);
            if (periodoEscolar == null)
                throw new NegocioException($"Período escolar do {bimestre}º Bimestre não localizado para a turma");

            return periodoEscolar;
        }

        private async Task<Turma> BuscaTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            return turma;
        }


        private async Task<string> CalculoFrequenciaGlobal2020(string alunoCodigo, Turma turma)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            var periodos = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);
            var disciplinasDaTurmaEol = await servicoEOL.ObterDisciplinasPorCodigoTurma(turma.CodigoTurma);
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


        public async Task<IEnumerable<AusenciaMotivoDto>> ObterAusenciaMotivoPorAlunoTurmaBimestreAno(string codigoAluno, string codigoTurma, short bimestre, short anoLetivo)
        => await mediator
            .Send(
                new ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery(
                    codigoAluno,
                    codigoTurma,
                    bimestre,
                    anoLetivo
                    )
            );

        public async Task<string> ObterFrequenciaGeralAlunoPorTurmas(string alunoCodigo, string[] turmasCodigos, string componenteCurricularCodigo = "", Turma turmaConsultada = null)
        {
            var turma = turmaConsultada ??
                await ObterTurma(turmasCodigos);

            //Particularidade de 2020
            if (turma.AnoLetivo.Equals(2020))
                return await CalculoFrequenciaGlobal2020(alunoCodigo, turma);

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            return await mediator.Send(new ObterFrequenciaGeralAlunoPorTurmasQuery(alunoCodigo, turmasCodigos, tipoCalendarioId));

        }

        private async Task<Turma> ObterTurma(string[] turmasCodigos)
        {
            var turmaCodigo = turmasCodigos.First();
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException("Turma não localizada.");

            return turma;
        }
    }
}