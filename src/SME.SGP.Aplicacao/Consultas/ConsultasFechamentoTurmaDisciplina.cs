﻿using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoTurmaDisciplina : IConsultasFechamentoTurmaDisciplina
    {
        private readonly IConsultasAulaPrevista consultasAulaPrevista;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasPeriodoFechamento consultasFechamento;
        private readonly IConsultasFechamentoNota consultasFechamentoNota;
        private readonly IConsultasFechamentoAluno consultasFehcamentoAluno;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioSintese repositorioSintese;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasFechamentoTurmaDisciplina(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
            IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioTurma repositorioTurma,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IConsultasFrequencia consultasFrequencia,
            IConsultasAulaPrevista consultasAulaPrevista,
            IConsultasPeriodoEscolar consultasPeriodoEscolar,
            IConsultasFechamentoNota consultasFechamentoNota,
            IServicoEol servicoEOL,
            IServicoUsuario servicoUsuario,
            IServicoAluno servicoAluno,
            IRepositorioConceito repositorioConceito,
            IRepositorioSintese repositorioSintese,
            IRepositorioParametrosSistema repositorioParametrosSistema,
            IConsultasPeriodoFechamento consultasFechamento,
            IConsultasDisciplina consultasDisciplina,
            IConsultasFechamentoAluno consultasFechamentoAluno,
            IConsultasPeriodoFechamento consultasPeriodoFechamento,
            IConsultasTurma consultasTurma
            )
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.consultasAulaPrevista = consultasAulaPrevista ?? throw new ArgumentNullException(nameof(consultasAulaPrevista));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasFechamentoNota = consultasFechamentoNota ?? throw new ArgumentNullException(nameof(consultasFechamentoNota));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.repositorioSintese = repositorioSintese ?? throw new ArgumentNullException(nameof(repositorioSintese));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasFechamento = consultasFechamento ?? throw new ArgumentNullException(nameof(consultasFechamento));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFehcamentoAluno = consultasFechamentoAluno ?? throw new ArgumentNullException(nameof(consultasFechamentoAluno));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
        }

        public IEnumerable<Sintese> _sinteses { get; set; }

        public IEnumerable<Sintese> Sinteses
        {
            get
            {
                if (_sinteses == null)
                    _sinteses = repositorioSintese.Listar();

                return _sinteses;
            }
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> ObterDadosAlunos(string turmaCodigo, int anoLetivo, int semestre)
        {
            var turma = await consultasTurma.ObterPorCodigo(turmaCodigo);
            var periodosAberto = await consultasPeriodoFechamento.ObterPeriodosComFechamentoEmAberto(turma.UeId);

            PeriodoEscolar periodoEscolar;
            if (periodosAberto != null && periodosAberto.Any())
            {
                // caso tenha mais de um periodo em aberto (abertura e reabertura) usa o ultimo bimestre
                periodoEscolar = periodosAberto.OrderBy(c => c.Bimestre).Last();
            }
            else
            {
                // Caso não esteja em periodo de fechamento ou escolar busca o ultimo existente
                var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, semestre);
                if (tipoCalendario == null)
                    throw new NegocioException("Não foi encontrado calendário cadastrado para a turma");
                var periodosEscolares = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);
                if (periodosEscolares == null)
                    throw new NegocioException("Não foram encontrados periodos escolares cadastrados para a turma");

                periodoEscolar = consultasPeriodoEscolar.ObterPeriodoPorData(periodosEscolares, DateTime.Today);
                if (periodoEscolar == null)
                    periodoEscolar = consultasPeriodoEscolar.ObterUltimoPeriodoPorData(periodosEscolares, DateTime.Today);
            }

            var dadosAlunos = await consultasTurma.ObterDadosAlunos(turmaCodigo, anoLetivo, periodoEscolar);
            return dadosAlunos.OrderBy(w => w.Nome);
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre)
                    => await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turmaId, disciplinaId, bimestre);

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplina(string turmaCodigo, string disciplinaId, int bimestre)
                    => await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turmaCodigo, new long[] { Convert.ToInt64(disciplinaId) }, bimestre);

        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
            => await repositorioFechamentoTurmaDisciplina.ObterNotasBimestre(codigoAluno, fechamentoTurmaId);

        public async Task<FechamentoTurmaDisciplinaBimestreDto> ObterNotasFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int? bimestre, int semestre)
        {
            var turma = await repositorioTurma.ObterPorCodigo(turmaId);
            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, ModalidadeParaModalidadeTipoCalendario(turma.ModalidadeCodigo), semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var bimestreAtual = bimestre;
            if (!bimestreAtual.HasValue || bimestre == 0)
                bimestreAtual = ObterBimestreAtual(periodosEscolares);

            var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestreAtual);
            if (periodoAtual == null)
                throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

            // Carrega alunos
            var alunos = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma, turma.AnoLetivo);
            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            // DTO de retorno
            var fechamentoBimestre = new FechamentoTurmaDisciplinaBimestreDto()
            {
                Bimestre = bimestreAtual.Value,
                Periodo = tipoCalendario.Periodo,
                TotalAulasDadas = 0, // Carregar
                TotalAulasPrevistas = 0, // Carregar
                Alunos = new List<NotaConceitoAlunoBimestreDto>()
            };

            var disciplinaEOL = await consultasDisciplina.ObterDisciplina(disciplinaId);
            IEnumerable<DisciplinaResposta> disciplinasRegencia = null;

            if (disciplinaEOL.Regencia)
                disciplinasRegencia = await servicoEOL.ObterDisciplinasParaPlanejamento(long.Parse(turmaId), servicoUsuario.ObterLoginAtual(), servicoUsuario.ObterPerfilAtual());

            fechamentoBimestre.EhSintese = !disciplinaEOL.LancaNota;

            // Carrega fechamento da Turma x Disciplina x Bimestre  
            var fechamentosTurma = await ObterFechamentosTurmaDisciplina(turmaId, disciplinaId.ToString(), bimestreAtual.Value);            
            if ((fechamentosTurma != null && fechamentosTurma.Any()) || fechamentoBimestre.EhSintese)
            {
                if (fechamentosTurma != null && fechamentosTurma.Any())
                {                     
                    fechamentoBimestre.Situacao = fechamentosTurma.First().Situacao;
                    fechamentoBimestre.SituacaoNome = fechamentosTurma.First().Situacao.Name();
                    fechamentoBimestre.FechamentoId = fechamentosTurma.First().Id;
                    fechamentoBimestre.DataFechamento = fechamentosTurma.First().AlteradoEm.HasValue ? fechamentosTurma.First().AlteradoEm.Value : fechamentosTurma.First().CriadoEm;
                }

                fechamentoBimestre.Alunos = new List<NotaConceitoAlunoBimestreDto>();

                var bimestreDoPeriodo = await consultasPeriodoEscolar.ObterPeriodoEscolarPorData(tipoCalendario.Id, periodoAtual.PeriodoFim);

                foreach (var aluno in alunos.Where(a => a.NumeroAlunoChamada > 0 || a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo)).OrderBy(a => a.NumeroAlunoChamada).ThenBy(a => a.NomeValido()))
                {
                    var fechamentoTurma = (from ft in fechamentosTurma
                                           from fa in ft.FechamentoAlunos
                                           where fa.AlunoCodigo.Equals(aluno.CodigoAluno)
                                           select ft).FirstOrDefault();

                    var alunoDto = new NotaConceitoAlunoBimestreDto();
                    alunoDto.CodigoAluno = aluno.CodigoAluno;
                    alunoDto.NumeroChamada = aluno.NumeroAlunoChamada;
                    alunoDto.Nome = aluno.NomeAluno;
                    alunoDto.Ativo = aluno.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo);

                    var anotacaoAluno = await consultasFehcamentoAluno.ObterAnotacaoPorAlunoEFechamento(fechamentoTurma?.Id ?? 0, aluno.CodigoAluno);
                    alunoDto.TemAnotacao = anotacaoAluno != null && anotacaoAluno.Anotacao != null &&
                                        !string.IsNullOrEmpty(anotacaoAluno.Anotacao.Trim());

                    var marcador = await servicoAluno.ObterMarcadorAluno(aluno, bimestreDoPeriodo);
                    if (marcador != null)
                    {
                        alunoDto.Informacao = marcador.Descricao;
                    }

                    var frequenciaAluno = consultasFrequencia.ObterPorAlunoDisciplinaData(aluno.CodigoAluno, disciplinaId.ToString(), periodoAtual.PeriodoFim);
                    if (frequenciaAluno != null)
                    {
                        alunoDto.QuantidadeFaltas = frequenciaAluno.TotalAusencias;
                        alunoDto.QuantidadeCompensacoes = frequenciaAluno.TotalCompensacoes;
                        alunoDto.PercentualFrequencia = frequenciaAluno.PercentualFrequencia;
                    }
                    else
                    {
                        // Quando não tem registro de frequencia assume 100%
                        alunoDto.QuantidadeFaltas = 0;
                        alunoDto.QuantidadeCompensacoes = 0;
                        alunoDto.PercentualFrequencia = 100;
                    }

                    // Carrega Frequencia do aluno
                    if (aluno.CodigoAluno != null)
                    {
                        if (fechamentoBimestre.EhSintese && fechamentoTurma == null)
                        {
                            var sinteseDto = await consultasFrequencia.ObterSinteseAluno(alunoDto.PercentualFrequencia, disciplinaEOL);

                            alunoDto.SinteseId = sinteseDto.Id;
                            alunoDto.Sintese = sinteseDto.Valor;
                        }
                        else
                        {
                            // Carrega notas do bimestre
                            var notasConceitoBimestre = await ObterNotasBimestre(aluno.CodigoAluno, fechamentoTurma != null ? fechamentoTurma.Id : 0);

                            if (notasConceitoBimestre.Any())
                                alunoDto.Notas = new List<FechamentoNotaRetornoDto>();

                            if (fechamentoBimestre.EhSintese)
                            {
                                var notaConceitoBimestre = notasConceitoBimestre.FirstOrDefault();
                                if (notaConceitoBimestre != null)
                                {
                                    alunoDto.SinteseId = (SinteseEnum)notaConceitoBimestre.SinteseId.Value;
                                    alunoDto.Sintese = ObterSintese(notaConceitoBimestre.SinteseId.Value);
                                }
                            }
                            else
                                foreach (var notaConceitoBimestre in notasConceitoBimestre)
                                {
                                    string nomeDisciplina;
                                    if (disciplinaEOL.Regencia)
                                        nomeDisciplina = disciplinasRegencia.FirstOrDefault(a => a.CodigoComponenteCurricular == notaConceitoBimestre.DisciplinaId)?.Nome;
                                    else nomeDisciplina = disciplinaEOL.Nome;

                                    ((List<FechamentoNotaRetornoDto>)alunoDto.Notas).Add(new FechamentoNotaRetornoDto()
                                    {
                                        DisciplinaId = notaConceitoBimestre.DisciplinaId,
                                        Disciplina = nomeDisciplina,
                                        NotaConceito = notaConceitoBimestre.ConceitoId.HasValue ? ObterConceito(notaConceitoBimestre.ConceitoId.Value) : notaConceitoBimestre.Nota,
                                        EhConceito = notaConceitoBimestre.ConceitoId.HasValue,
                                        ConceitoDescricao = notaConceitoBimestre.ConceitoId.HasValue ? ObterConceitoDescricao(notaConceitoBimestre.ConceitoId.Value) : string.Empty
                                    });
                                }
                        }

                        fechamentoBimestre.Alunos.Add(alunoDto);
                    }
                }
            }

            var aulaPrevisa = await consultasAulaPrevista.ObterAulaPrevistaDada(turma.ModalidadeCodigo, turma.CodigoTurma, disciplinaId.ToString(), semestre);
            var aulaPrevistaBimestreAtual = new AulasPrevistasDadasDto();
            if (aulaPrevisa != null)
            {
                aulaPrevistaBimestreAtual = aulaPrevisa.AulasPrevistasPorBimestre.FirstOrDefault(a => a.Bimestre == bimestreAtual);
            }

            fechamentoBimestre.Bimestre = bimestreAtual.Value;
            fechamentoBimestre.TotalAulasDadas = aulaPrevistaBimestreAtual.Cumpridas;
            fechamentoBimestre.TotalAulasPrevistas = aulaPrevistaBimestreAtual.Previstas.Quantidade;

            fechamentoBimestre.PodeProcessarReprocessar = await consultasFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, DateTime.Today, bimestreAtual.Value);

            return fechamentoBimestre;
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }

        private int ObterBimestreAtual(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dataPesquisa = DateTime.Now;

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar == null)
                return 1;
            else return periodoEscolar.Bimestre;
        }

        private double ObterConceito(long id)
        {
            var conceito = repositorioConceito.ObterPorId(id);
            return conceito != null ? conceito.Id : 0;
        }

        private string ObterConceitoDescricao(long id)
        {
            var conceito = repositorioConceito.ObterPorId(id);
            return conceito != null ? conceito.Valor : "";
        }

        private string ObterSintese(long id)
        {
            var sintese = Sinteses.FirstOrDefault(c => c.Id == id);
            return sintese != null ? sintese.Descricao : "";
        }
    }
}