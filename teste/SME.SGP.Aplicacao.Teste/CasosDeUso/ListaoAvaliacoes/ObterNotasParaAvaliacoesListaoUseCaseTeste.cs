using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ListaoAvaliacoes
{
    public class ObterNotasParaAvaliacoesListaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConsultasDisciplina> _consultasDisciplinaMock;
        private readonly Mock<IConsultasPeriodoFechamento> _consultasPeriodoFechamentoMock;
        private readonly ObterNotasParaAvaliacoesListaoUseCase _useCase;

        public ObterNotasParaAvaliacoesListaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _consultasDisciplinaMock = new Mock<IConsultasDisciplina>();
            _consultasPeriodoFechamentoMock = new Mock<IConsultasPeriodoFechamento>();

            _useCase = new ObterNotasParaAvaliacoesListaoUseCase(
                _mediatorMock.Object,
                _consultasDisciplinaMock.Object,
                _consultasPeriodoFechamentoMock.Object);
        }
        private ListaNotasConceitosBimestreRefatoradaDto CriarFiltroSimples() =>
            new ListaNotasConceitosBimestreRefatoradaDto
            {
                TurmaCodigo = "1",
                DisciplinaCodigo = 1,
                PeriodoInicioTicks = DateTime.Today.AddMonths(-1).Ticks,
                PeriodoFimTicks = DateTime.Today.Ticks,
                Bimestre = 1,
                AnoLetivo = DateTime.Today.Year,
                TurmaHistorico = false
            };

        [Fact]
        public async Task Nao_Permite_Editar_Nota_Avaliacao_Com_Data_Inferior_Data_Matricula_Aluno()
        {
            var filtro = CriarFiltroSimples();
            var anoAtual = DateTime.Today.Year;

            var periodoInicio = new DateTime(anoAtual, 2, 1);
            var periodoFim = new DateTime(anoAtual, 4, 30);
            var dataMatricula = new DateTime(anoAtual, 3, 15);

            var atividadeAntesMatricula = new AtividadeAvaliativa { Id = 1, DataAvaliacao = new DateTime(anoAtual, 3, 7) };   // Deve ser NÃO editável
            var atividadeDepoisMatricula = new AtividadeAvaliativa { Id = 2, DataAvaliacao = new DateTime(anoAtual, 4, 24) }; // Deve ser editável

            var codigoTurma = "1";
            var codigoAluno = "1";

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = codigoTurma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "rf", PerfilAtual = Dominio.Perfis.PERFIL_CJ });

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(codigoTurma, true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto> { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(y =>
                y.ComponentesCurriculares.Single() == "1" &&
                y.TurmaCodigo == codigoTurma &&
                y.PeriodoInicio == periodoInicio &&
                y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa> { atividadeAntesMatricula, atividadeDepoisMatricula });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>
                {
                  new() { CodigoAluno = codigoAluno, CodigoTurma = Convert.ToInt64(codigoTurma), DataMatricula = dataMatricula, CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("30");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(y =>
                y.Ids.Single() == 1 && y.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto> { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>
                {
                   new() { CodigoAluno = codigoAluno, CodigoTurma = Convert.ToInt64(codigoTurma), NomeAluno = "Joaozinho", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 1, DataMatricula = dataMatricula }
                });

            var resultado = await _useCase.Executar(new ListaNotasConceitosBimestreRefatoradaDto
            {
                AnoLetivo = anoAtual,
                Bimestre = 1,
                DisciplinaCodigo = 1,
                Modalidade = Modalidade.Fundamental,
                TurmaCodigo = codigoTurma,
                TurmaId = 1,
                PeriodoInicioTicks = periodoInicio.Ticks,
                PeriodoFimTicks = periodoFim.Ticks,
                PeriodoEscolarId = 1,
            });

            var aluno = resultado.Bimestres.Single().Alunos.Single();
            Assert.NotNull(resultado);
            Assert.Single(resultado.Bimestres);
            Assert.Single(resultado.Bimestres.Single().Alunos);
            Assert.Equal(2, aluno.NotasAvaliacoes.Count);

            var notaAntesMatricula = aluno.NotasAvaliacoes.Single(na => na.AtividadeAvaliativaId == atividadeAntesMatricula.Id);
            var notaDepoisMatricula = aluno.NotasAvaliacoes.Single(na => na.AtividadeAvaliativaId == atividadeDepoisMatricula.Id);

            Assert.False(notaAntesMatricula.PodeEditar); 
            Assert.True(notaDepoisMatricula.PodeEditar);

            var matriculasAluno = resultado.Bimestres.Single().Alunos;
        }

        [Fact]
        public async Task Deve_Desconsiderar_Alunos_Com_Vinculo_Indevido()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 01);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            var codigoTurma = "1";

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = codigoTurma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "rf", PerfilAtual = Dominio.Perfis.PERFIL_CJ });

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(codigoTurma, true, It.IsAny<bool>(), It.IsAny<bool>()))
                 .ReturnsAsync(new List<DisciplinaDto> { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(y =>
                y.ComponentesCurriculares.Single() == "1" &&
                y.TurmaCodigo == codigoTurma &&
                y.PeriodoInicio == periodoInicio &&
                y.PeriodoFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>() {
            new() { Id = 1, DataAvaliacao = new DateTime(anoAtual, 03, 07) },
            new() { Id = 2, DataAvaliacao = new DateTime(anoAtual, 04, 24) } });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(q =>
                    q.CodigoTurma == codigoTurma &&
                    q.Periodo.Item1 == periodoInicio &&
                    q.Periodo.Item2 == periodoFim),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>()
                {
            new() { CodigoAluno = "1", DataMatricula = new DateTime(anoAtual, 03, 15), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 1 },
            new() { CodigoAluno = "2", DataMatricula = new DateTime(anoAtual, 01, 10), CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido, NumeroAlunoChamada = 2 },
            new() { CodigoAluno = "3", DataMatricula = new DateTime(anoAtual - 1, 11, 01), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 3 }
                });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(y =>
                y.Ids.Single() == 1 && y.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y =>
                y.Tipo == TipoParametroSistema.MediaBimestre && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y =>
                y.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes && y.Ano == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync("50");

            _mediatorMock.Setup(x => x.Send(It.Is<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(y =>
                y.DataAvaliacao == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterNotasPorAlunosAtividadesAvaliativasQuery>(y =>
                y.AtividadesAvaliativasId.Intersect(new long[] { 1, 2 }).Count() == 2 &&
                y.AlunosId.Intersect(new string[] { "1", "3" }).Count() == 2 &&
                y.ComponenteCurricularId == "1" &&
                y.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAusenciasDaAtividadesAvaliativasQuery>(y =>
                y.TurmaCodigo == codigoTurma && y.ComponenteCurricularCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            var fechamentos = new List<FechamentoPorTurmaPeriodoCCDto>() { new() };
            fechamentos[0].FechamentoAlunos.Add(new() { AlunoCodigo = "1" });
            fechamentos[0].FechamentoAlunos[0].FechamentoNotas.Add(new FechamentoNotaPorTurmaPeriodoCCDto() { Id = 1 });

            _mediatorMock.Setup(x => x.Send(It.Is<ObterFechamentosPorTurmaPeriodoCCQuery>(y =>
                y.PeriodoEscolarId == 1 &&
                y.TurmaId == 1 &&
                y.ComponenteCurricularId == 1 &&
                !y.EhRegencia), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentos);

            _mediatorMock.Setup(x => x.Send(It.Is<ObterNotaEmAprovacaoPorFechamentoNotaIdQuery>(y =>
                y.IdsFechamentoNota.Single() == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaAprovacaoDto>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterFrequenciasPorAlunosTurmaCCDataQuery>(y =>
                y.AlunosCodigo.Intersect(new string[] { "1", "3" }).Count() == 1 &&
                y.DataReferencia == periodoFim &&
                y.TipoFrequencia == TipoFrequenciaAluno.PorDisciplina &&
                y.TurmaCodigo == codigoTurma &&
                y.ComponenteCurriularId == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FrequenciaAluno>() { new() { CodigoAluno = "1" } });

            _consultasPeriodoFechamentoMock.Setup(x => x.TurmaEmPeriodoDeFechamentoVigente(It.IsAny<Turma>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new PeriodoFechamentoVigenteDto());

            _mediatorMock.Setup(x => x.Send(It.Is<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(y =>
                y.CodigoEstudante.Intersect(new string[] { "1", "3" }).Count() == 2 && y.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto>());

            _mediatorMock.Setup(x => x.Send(It.Is<ObterAlunosAtivosTurmaProgramaPapEolQuery>(y =>
                y.AnoLetivo == anoAtual && y.AlunosCodigos.Intersect(new string[] { "1", "3" }).Count() == 2), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunosTurmaProgramaPapDto>());

            var resultado = await _useCase.Executar(new ListaNotasConceitosBimestreRefatoradaDto()
            {
                AnoLetivo = anoAtual,
                Bimestre = 1,
                DisciplinaCodigo = 1,
                Modalidade = Modalidade.Fundamental,
                TurmaCodigo = codigoTurma,
                TurmaId = 1,
                PeriodoInicioTicks = periodoInicio.Ticks,
                PeriodoFimTicks = periodoFim.Ticks,
                PeriodoEscolarId = 1
            });

            Assert.NotNull(resultado);
            Assert.Single(resultado.Bimestres);
            Assert.Equal(2, resultado.Bimestres.Single().Alunos.Count);
            Assert.Single(resultado.Bimestres.Single().Alunos.Where(a => a.NumeroChamada == 1));
            Assert.Empty(resultado.Bimestres.Single().Alunos.Where(a => a.NumeroChamada == 2));
            Assert.Single(resultado.Bimestres.Single().Alunos.Where(a => a.NumeroChamada == 3));
        }

        [Fact]
        public async Task Executar_Retorna_Dados_Corretamente()
        {
            var filtro = CriarFiltroSimples();

            var tipoCalendario = new SME.SGP.Dominio.TipoCalendario
            {
                AnoLetivo = filtro.AnoLetivo,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Semestral,
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = filtro.TurmaCodigo });

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa> {
                new AtividadeAvaliativa { Id = 1, DataAvaliacao = DateTime.Today, NomeAvaliacao = "Prova 1", TipoAvaliacaoId = 1, Categoria = CategoriaAtividadeAvaliativa.Normal }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> {
                new() { CodigoAluno = "1", CodigoTurma = 12345, NomeAluno = "Joaozinho", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 1 }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { Id = 1, AvaliacoesNecessariasPorBimestre = 1 });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("30");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito> {
                new NotaConceito { AlunoId = "123456", AtividadeAvaliativaID = 1, CriadoEm = DateTime.Now, CriadoPor = "user", CriadoRF = "rf" }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciasDaAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AusenciaAlunoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto> {
                new DisciplinaDto { CodigoComponenteCurricular = filtro.DisciplinaCodigo, Regencia = false }
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "rf", PerfilAtual = Dominio.Perfis.PERFIL_CJ });

            _consultasPeriodoFechamentoMock.Setup(c => c.TurmaEmPeriodoDeFechamento(It.IsAny<Turma>(), tipoCalendario, It.IsAny<DateTime>(), filtro.Bimestre))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarcadorFrequenciaDto
                {
                    Tipo = TipoMarcadorFrequencia.Novo,
                    Descricao = "MarcadorTeste"
                });

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoNotaPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TipoNota.Conceito);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado.Bimestres);
            Assert.Equal(filtro.Bimestre, resultado.BimestreAtual);
            Assert.NotNull(resultado.NotaTipo);
            Assert.Equal("MarcadorTeste", resultado.Bimestres.First().Alunos.First().Marcador.Descricao);
        }

        [Fact]
        public async Task Executar_Turma_Nao_Encontrada_Lanca_Negocio_Exception()
        {
            var filtro = CriarFiltroSimples();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }

        [Fact]
        public async Task Executar_Componentes_Curriculares_Do_Professor_Vazios_Lanca_NegocioException()
        {
            var filtro = CriarFiltroSimples();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = filtro.TurmaCodigo });

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true, It.IsAny<bool>(), It.IsAny<bool>()))
               .ReturnsAsync(new List<DisciplinaDto>());

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }

        [Fact]
        public async Task Executar_Alunos_Nao_Encontrados_Lanca_NegocioException()
        {
            var filtro = CriarFiltroSimples();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = filtro.TurmaCodigo });

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true, It.IsAny<bool>(), It.IsAny<bool>()))
               .ReturnsAsync(new List<DisciplinaDto> { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }

        [Fact]
        public async Task Executar_Componente_Curricular_Nao_Encontrado_Lanca_NegocioException()
        {
            var filtro = CriarFiltroSimples();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = filtro.TurmaCodigo });

            _consultasDisciplinaMock.Setup(c => c.ObterComponentesCurricularesPorProfessorETurma(filtro.TurmaCodigo, true, It.IsAny<bool>(), It.IsAny<bool>()))
               .ReturnsAsync(new List<DisciplinaDto> { new() { CodigoComponenteCurricular = 1 } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1", CodigoTurma = 12345, NomeAluno = "Joaozinho", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, NumeroAlunoChamada = 1 } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("30");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>());

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }
    }
}
