using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ConsultaConselhoClasseRecomendacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IConsultaConselhoClasseRecomendacaoUseCase _useCase;

        private readonly ConselhoClasseRecomendacaoDto _recomendacaoDto;
        private readonly Turma _turma;

        public ConsultaConselhoClasseRecomendacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsultaConselhoClasseRecomendacaoUseCase(_mediatorMock.Object);

            _recomendacaoDto = new ConselhoClasseRecomendacaoDto
            {
                AlunoCodigo = "123",
                Bimestre = 1,
                CodigoTurma = "T1",
                ConselhoClasseId = 1,
                FechamentoTurmaId = 1
            };

            _turma = new Turma
            {
                Id = 1,
                CodigoTurma = "T1",
                AnoLetivo = DateTime.Now.Year,
                ModalidadeCodigo = Modalidade.Fundamental,
                TipoTurma = TipoTurma.Regular,
                Semestre = 1
            };
        }

        [Fact]
        public void Executar_QuandoMediatorNulo_DeveLancarArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultaConselhoClasseRecomendacaoUseCase(null));
        }

        [Fact]
        public async Task Executar_QuandoTurmaNula_DeveLancarNullReferenceException()
        {
            var recomendacaoDto = new ConselhoClasseRecomendacaoDto { CodigoTurma = "T1" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(recomendacaoDto));
        }

        [Fact]
        public async Task Executar_QuandoPeriodoEscolarNaoEncontrado_DeveLancarNegocioException()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((FechamentoTurma)null);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((PeriodoEscolar)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(_recomendacaoDto));
            Assert.Equal(MensagemNegocioPeriodo.PERIODO_ESCOLAR_NAO_ENCONTRADO, exception.Message);
        }

        [Fact]
        public async Task Executar_QuandoTipoCalendarioNaoEncontrado_DeveLancarNegocioException()
        {
            SetupMocksPadrao();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((SME.SGP.Dominio.TipoCalendario)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(_recomendacaoDto));
            Assert.Equal(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO, exception.Message);
        }

        [Fact]
        public async Task Executar_QuandoPeriodosLetivosNaoEncontrados_DeveLancarNegocioException()
        {
            SetupMocksPadrao();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<PeriodoEscolar>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(_recomendacaoDto));
            Assert.Equal(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TIPO_CALENDARIO, exception.Message);
        }

        [Fact]
        public async Task Executar_QuandoConselhoFinalSemBimestreAnterior_DeveLancarNegocioException()
        {
            var recomendacaoDtoFinal = new ConselhoClasseRecomendacaoDto
            {
                Bimestre = 0, 
                AlunoCodigo = "123",
                CodigoTurma = "T1",
                FechamentoTurmaId = 9,
                ConselhoClasseId = 8
            };

            var turma = new Turma
            {
                AnoLetivo = 2021,
                Historica = false,
                CodigoTurma = "T1",
                Id = 99,
                TipoTurma = TipoTurma.Regular, 
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var fechamentoTurma = new FechamentoTurma { Turma = turma };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(fechamentoTurma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new[] { turma.CodigoTurma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseIdsPorTurmaEPeriodoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new long[] { 1 });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new SME.SGP.Dominio.TipoCalendario { Id = 1 });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<PeriodoEscolar> { new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(1) } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComMatriculasValidasPeriodoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<string> { turma.CodigoTurma });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUltimoBimestreTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((bimestre: 4, possuiConselho: false));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ConselhoClasseAluno)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(recomendacaoDtoFinal));

            Assert.Equal(string.Format(MensagemNegocioConselhoClasse.PARA_ACESSAR_ESTA_ABA_E_PRECISO_REGISTRAR_O_CONSELHO_DE_CLASSE_DO_X_BIMESTRE, 4), exception.Message);
        }

        [Fact]
        public async Task Executar_QuandoFluxoCompleto_DeveRetornarDtoPreenchido()
        {
            SetupMocksPadrao();
            _turma.TipoTurma = TipoTurma.EdFisica;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new[] { "T1", "T2" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseIdsPorTurmaEPeriodoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new long[] { 10, 20 });

            var conselhoAluno = new ConselhoClasseAluno
            {
                RecomendacoesAluno = "Recomendação Aluno",
                RecomendacoesFamilia = "Recomendação Família",
                AnotacoesPedagogicas = "Anotação Pedagógica",
                CriadoEm = DateTime.Now.AddDays(-1),
                AlteradoEm = DateTime.Now
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(conselhoAluno);

            var resultado = await _useCase.Executar(_recomendacaoDto);

            Assert.NotNull(resultado);
            Assert.Equal("Recomendação Aluno\r\n", resultado.TextoRecomendacaoAluno);
            Assert.Equal("Recomendação Família\r\n", resultado.TextoRecomendacaoFamilia);
            Assert.Equal("Anotação Pedagógica\r\n", resultado.AnotacoesPedagogicas);
            Assert.NotNull(resultado.Auditoria);
            Assert.False(resultado.SomenteLeitura);
        }

        [Fact]
        public async Task Executar_QuandoTurmaNaoRegular_DeveSeguirFluxoAlternativo()
        {
            _turma.TipoTurma = TipoTurma.Programa;
            SetupMocksPadrao();

            var resultado = await _useCase.Executar(_recomendacaoDto);

            Assert.NotNull(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()), Moq.Times.Never);
        }

        [Fact]
        public async Task Executar_QuandoConselhoAlunoNulo_DeveContinuarLoop()
        {
            SetupMocksPadrao();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseIdsPorTurmaEPeriodoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new long[] { 10, 20 });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ConselhoClasseAluno)null);

            var resultado = await _useCase.Executar(_recomendacaoDto);

            Assert.NotNull(resultado);
            Assert.Empty(resultado.TextoRecomendacaoAluno);
            Assert.Null(resultado.Auditoria);
        }

        private void SetupMocksPadrao(bool comFechamento = true)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_turma);

            var periodoEscolar = new PeriodoEscolar { Id = 1, PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddMonths(2) };
            if (comFechamento)
            {
                var fechamentoTurma = new FechamentoTurma { PeriodoEscolar = periodoEscolar, Turma = _turma };
                _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(fechamentoTurma);
            }
            else
            {
                _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((FechamentoTurma)null);
            }

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto>());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new[] { "T1" });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseIdsPorTurmaEPeriodoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new long[0]);

            var tipoCalendario = new SME.SGP.Dominio.TipoCalendario { Id = 1 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(tipoCalendario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<PeriodoEscolar> { periodoEscolar });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComMatriculasValidasPeriodoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<string> { "T1" });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaEmPeriodoDeFechamentoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoAlunoParaConselhoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<FechamentoAlunoAnotacaoConselhoDto>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ConselhoClasseAluno());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterRecomendacoesPorAlunoConselhoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<RecomendacoesAlunoFamiliaDto>());

            var consolidado = new ConselhoClasseConsolidadoTurmaAluno() { Status = SituacaoConselhoClasse.EmAndamento };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(consolidado);
        }
    }
}
