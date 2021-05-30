using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
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
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoFrequencia servicoFrequencia;
        private readonly IMediator mediator;

        private double _mediaFrequencia;

        public ConsultasFrequencia(IMediator mediator,
                                   IServicoFrequencia servicoFrequencia,
                                   IServicoEol servicoEOL,
                                   IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                    IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                   IConsultasTipoCalendario consultasTipoCalendario,
                                   IConsultasTurma consultasTurma,
                                   IRepositorioAula repositorioAula,
                                   IRepositorioFrequencia repositorioFrequencia,
                                   IRepositorioTurma repositorioTurma,
                                   IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                   IRepositorioParametrosSistema repositorioParametrosSistema,
                                   IServicoAluno servicoAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoFrequencia = servicoFrequencia ?? throw new ArgumentNullException(nameof(servicoFrequencia));
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
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<bool> FrequenciaAulaRegistrada(long aulaId)
            => await repositorioFrequencia.FrequenciaAulaRegistrada(aulaId);

        public async Task<double?> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "")
        {

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            var tipoCalendarioId = turma.ModalidadeCodigo == Modalidade.EJA ? await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma)) : 0 ;
            
            var frequenciaAluno = await mediator.Send(new ObterFrequenciaGeralAlunoPorCodigoAnoSemestreQuery(alunoCodigo, turma.AnoLetivo, tipoCalendarioId));
            
            if (frequenciaAluno == null)
                return null;

            //Particularidade de 2020
            if (turma.AnoLetivo.Equals(2020))
                return await CalculoFrequenciaGlobal2020(alunoCodigo, turma);

            return frequenciaAluno.PercentualFrequencia;
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

            var turma = await repositorioTurma.ObterPorCodigo(turmaCodigo);
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            var periodos = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);

            periodos.ToList().ForEach(p =>
            {
                var frequenciaCorrespondente = frequenciaAlunoPeriodos.SingleOrDefault(f => f.Bimestre == p.Bimestre);
                frequenciaAluno.AdicionarFrequenciaBimestre(p.Bimestre, frequenciaCorrespondente != null ? frequenciaCorrespondente.PercentualFrequencia : 100);
            });

            return frequenciaAluno;
        }

        public async Task<double> ObterFrequenciaMedia(DisciplinaDto disciplina)
        {
            if (_mediaFrequencia == 0)
            {
                if (disciplina.Regencia || !disciplina.LancaNota)
                    _mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse, DateTime.Today.Year)));
                else
                    _mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualFund2, DateTime.Today.Year)));
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

            var alunosEOL = await servicoEOL.ObterAlunosPorTurma(turmaId);
            if (alunosEOL == null || !alunosEOL.Any())
                throw new NegocioException("Não foram localizados alunos para a turma selecionada.");

            var disciplinasEOL = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { long.Parse(disciplinaId) });
            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Componente curricular informado não localizado no EOL.");

            var quantidadeMaximaCompensacoes = int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia, DateTime.Today.Year)));
            var percentualFrequenciaAlerta = int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(disciplinasEOL.First().Regencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2, DateTime.Today.Year)));

            var alunosAtivos = alunosEOL.Where(a => a.CodigoSituacaoMatricula != SituacaoMatriculaAluno.RemanejadoSaida);
            foreach (var alunoEOL in alunosAtivos)
            {
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaData(alunoEOL.CodigoAluno, disciplinaId, periodo.PeriodoFim);
                if (frequenciaAluno == null || frequenciaAluno.NumeroFaltasNaoCompensadas == 0)
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

        public async Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId, long? disciplinaId = null)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
                throw new NegocioException("Aula não encontrada.");



            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(aula.TurmaId);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");

            var turma = await repositorioTurma.ObterPorCodigo(aula.TurmaId);
            if (turma == null)
                throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");

            FrequenciaDto registroFrequenciaDto = await ObterRegistroFrequencia(aulaId, aula, turma);

            var ausencias = servicoFrequencia.ObterListaAusenciasPorAula(aulaId);
            if (ausencias == null)
                ausencias = new List<RegistroAusenciaAluno>();

            var bimestre = await consultasPeriodoEscolar.ObterPeriodoEscolarPorData(aula.TipoCalendarioId, aula.DataAula);
            if (bimestre == null)
                throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");


            registroFrequenciaDto.TemPeriodoAberto = await consultasTurma.TurmaEmPeriodoAberto(aula.TurmaId, DateTime.Today, bimestre.Bimestre);

            var parametroPercentualCritico = await repositorioParametrosSistema.ObterValorPorTipoEAno(
                                                    TipoParametroSistema.PercentualFrequenciaCritico,
                                                    bimestre.PeriodoInicio.Year);
            if (parametroPercentualCritico == null)
                throw new NegocioException("Parâmetro de percentual de frequência em nível crítico não encontrado contate a SME.");

            var percentualCritico = int.Parse(parametroPercentualCritico);
            var percentualAlerta = int.Parse(await repositorioParametrosSistema.ObterValorPorTipoEAno(
                                                    TipoParametroSistema.PercentualFrequenciaAlerta,
                                                    bimestre.PeriodoInicio.Year));

            var disciplinaAula = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId.HasValue ? disciplinaId.Value : Convert.ToInt64(aula.DisciplinaId) });

            if (disciplinaAula == null || disciplinaAula.ToList().Count <= 0)
                throw new NegocioException("Componente curricular da aula não encontrado");

            var anotacoesTurma = await mediator.Send(new ObterAlunosComAnotacaoNaAulaQuery(aulaId));

            foreach (var aluno in alunosDaTurma.Where(a => a.DeveMostrarNaChamada(aula.DataAula)).OrderBy(c => c.NomeAluno))
            {
                // Apos o bimestre da inatividade o aluno não aparece mais na lista de frequencia ou
                // se a matrícula foi ativada após a data da aula                
                if ((aluno.EstaInativo(aula.DataAula) && aluno.DataSituacao < bimestre.PeriodoInicio) ||
                    (aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo && aluno.DataSituacao > aula.DataAula))
                    continue;

                if (aula.DataAula < aluno.DataMatricula.Date)
                    continue;

                if (aluno.EstaInativo(aula.DataAula) && aluno.DataSituacao < aula.DataAula)
                    continue;

                var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));
                var registroFrequenciaAluno = new RegistroFrequenciaAlunoDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.NumeroAlunoChamada,
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    DataSituacao = aluno.DataSituacao,
                    DataNascimento = aluno.DataNascimento,
                    Desabilitado = aluno.EstaInativo(aula.DataAula) || aula.EhDataSelecionadaFutura,
                    PermiteAnotacao = aluno.EstaAtivo(aula.DataAula),
                    PossuiAnotacao = anotacoesTurma.Any(a => a == aluno.CodigoAluno),
                    NomeResponsavel = aluno.NomeResponsavel,
                    TipoResponsavel = ObterTipoResponsavel(aluno.TipoResponsavel),
                    CelularResponsavel = aluno.CelularResponsavel,
                    DataAtualizacaoContato = aluno.DataAtualizacaoContato,
                    EhAtendidoAEE = alunoPossuiPlanoAEE
                };

                // Marcador visual da situação
                registroFrequenciaAluno.Marcador = servicoAluno.ObterMarcadorAluno(aluno, bimestre, turma.EhTurmaInfantil);

                // Indicativo de frequencia do aluno
                aluno.CodigoTurma = long.Parse(turma.CodigoTurma);
                registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(aluno, aula.DisciplinaId, bimestre, percentualAlerta, percentualCritico);

                if (!disciplinaAula.FirstOrDefault().RegistraFrequencia)
                {
                    registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
                    continue;
                }

                var ausenciasAluno = ausencias.Where(c => c.CodigoAluno == aluno.CodigoAluno);

                for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
                {
                    registroFrequenciaAluno.Aulas.Add(new FrequenciaAulaDto
                    {
                        NumeroAula = numeroAula,
                        Compareceu = !ausenciasAluno.Any(c => c.NumeroAula == numeroAula)
                    });
                }

                registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
            }

            registroFrequenciaDto.Desabilitado = registroFrequenciaDto.ListaFrequencia.All(c => c.Desabilitado) || aula.EhDataSelecionadaFutura;

            return registroFrequenciaDto;
        }

        public FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual)
            => repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaData(codigoAluno, disciplinaId, dataAtual);

        public async Task<SinteseDto> ObterSinteseAluno(double? percentualFrequencia, DisciplinaDto disciplina)
        {
            var sintese = percentualFrequencia != null ? 
                SinteseEnum.NaoFrequente :
                percentualFrequencia >= await ObterFrequenciaMedia(disciplina) ?
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
            var turma = await repositorioTurma.ObterPorCodigo(turmaId);
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            return turma;
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(AlunoPorTurmaResposta aluno, string disciplinaId, PeriodoEscolar bimestre, int percentualAlerta, int percentualCritico)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(aluno.CodigoAluno, disciplinaId, bimestre.Id, TipoFrequenciaAluno.PorDisciplina, aluno.CodigoTurma.ToString());
            // Frequencia não calculada
            if (frequenciaAluno == null)
                return null;

            //if (frequenciaAluno == null)
            //{
            //    if (aluno.PodeEditarNotaConceito())
            //        return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Info, Percentual = 100 };

            //    return null;
            //}

            int percentualFrequencia = (int)Math.Round(frequenciaAluno.PercentualFrequencia, 0);
            // Critico
            if (percentualFrequencia <= percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequencia };

            // Alerta
            if (percentualFrequencia <= percentualAlerta)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Alerta, Percentual = percentualFrequencia };

            return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Info, Percentual = percentualFrequencia };
        }

        private async Task<FrequenciaDto> ObterRegistroFrequencia(long aulaId, Aula aula, Turma turma)
        {
            var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aulaId));
            if (registroFrequencia == null)
            {
                registroFrequencia = new RegistroFrequencia(aula);
            }
            var registroFrequenciaDto = new FrequenciaDto(aulaId)
            {
                AlteradoEm = registroFrequencia.AlteradoEm,
                AlteradoPor = registroFrequencia.AlteradoPor,
                AlteradoRF = registroFrequencia.AlteradoRF,
                CriadoEm = registroFrequencia.CriadoEm,
                CriadoPor = registroFrequencia.CriadoPor,
                CriadoRF = registroFrequencia.CriadoRF,
                Id = registroFrequencia.Id,
                Desabilitado = !aula.PermiteRegistroFrequencia(turma)
            };
            return registroFrequenciaDto;
        }

        private string ObterTipoResponsavel(string tipoResponsavel)
        {
            switch (tipoResponsavel)
            {
                case "1":
                    {
                        return TipoResponsavel.Filicacao1.Name();
                    }
                case "2":
                    {
                        return TipoResponsavel.Filiacao2.Name();
                    }
                case "3":
                    {
                        return TipoResponsavel.ResponsavelLegal.Name();
                    }
                case "4":
                    {
                        return TipoResponsavel.ProprioEstudante.Name();
                    }
            }
            return TipoResponsavel.Filicacao1.ToString();
        }


        private async Task<double> CalculoFrequenciaGlobal2020(string alunoCodigo, Turma turma)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            var periodos = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);
            var disciplinasDaTurmaEol = await servicoEOL.ObterDisciplinasPorCodigoTurma(turma.CodigoTurma);
            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray()));
            var gruposMatrizes = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatrizNome != null).GroupBy(c => c.GrupoMatrizNome).ToList();
            var somaFrequenciaFinal = 0.0;
            var totalDisciplinas = 0;

            foreach (var grupoDisiplinasMatriz in gruposMatrizes.OrderBy(k => k.Key))
            {
                foreach (var disciplina in grupoDisiplinasMatriz)
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
                totalDisciplinas += grupoDisiplinasMatriz.Count();
            }

            return Math.Round(somaFrequenciaFinal / totalDisciplinas, 2);
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
    }
}