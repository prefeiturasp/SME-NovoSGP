using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase _useCase;

        public ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarFrequenciasComDadosCompletos()
        {
            // Arrange
            var turmaCodigo = "123ABC";
            var alunoCodigo = "456DEF";
            var bimestres = new int[] { 1, 2 };
            var componenteCurricularId = "MAT";

            var dto = new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(turmaCodigo, alunoCodigo, bimestres, componenteCurricularId);

            var turmaMock = new Turma { CodigoTurma = turmaCodigo, Id = 10L };
            long tipoCalendarioIdMock = 100L;

            var frequenciasAlunoMock = new List<FrequenciaAluno>
            {
                new FrequenciaAluno
                {
                    CodigoAluno = alunoCodigo,
                    TurmaId = turmaCodigo,
                    DisciplinaId = componenteCurricularId,
                    Bimestre = 1,
                    TotalAulas = 20,
                    TotalAusencias = 2,
                    TotalCompensacoes = 1,
                    TotalPresencas = 17,
                    TotalRemotos = 0
                }
            };

            var aulasComponentesTurmasMock = new List<TurmaComponenteQntAulasDto>
            {
                new TurmaComponenteQntAulasDto
                {
                    TurmaCodigo = turmaCodigo,
                    ComponenteCurricularCodigo = componenteCurricularId,
                    Bimestre = 1,
                    AulasQuantidade = 20
                },
                new TurmaComponenteQntAulasDto
                {
                    TurmaCodigo = turmaCodigo,
                    ComponenteCurricularCodigo = componenteCurricularId,
                    Bimestre = 2,
                    AulasQuantidade = 25
                }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoCalendarioIdPorTurmaQuery>(q => q.Turma == turmaMock), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioIdMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(q =>
                q.AlunoCodigo == alunoCodigo &&
                q.ComponenteCurricularId == componenteCurricularId &&
                q.TurmaCodigo == turmaCodigo &&
                q.Bimestres.SequenceEqual(bimestres)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(frequenciasAlunoMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery>(q =>
                q.TurmasCodigo.Contains(turmaCodigo) &&
                q.TipoCalendarioId == tipoCalendarioIdMock &&
                q.ComponentesCurricularesId.Contains(componenteCurricularId) &&
                q.Bimestres.SequenceEqual(bimestres)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aulasComponentesTurmasMock);

            // Act
            var result = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var frequenciaBimestre1 = result.FirstOrDefault(f => f.Bimestre == 1);
            Assert.NotNull(frequenciaBimestre1);
            Assert.Equal(alunoCodigo, frequenciaBimestre1.CodigoAluno);
            Assert.Equal(componenteCurricularId, frequenciaBimestre1.DisciplinaId);
            Assert.Equal(turmaCodigo, frequenciaBimestre1.TurmaId);
            Assert.Equal(20, frequenciaBimestre1.TotalAulas);
            Assert.Equal(2, frequenciaBimestre1.TotalAusencias);
            Assert.Equal(1, frequenciaBimestre1.TotalCompensacoes);
            Assert.Equal(17, frequenciaBimestre1.TotalPresencas);
            Assert.Equal(0, frequenciaBimestre1.TotalRemotos);

            var frequenciaBimestre2 = result.FirstOrDefault(f => f.Bimestre == 2);
            Assert.NotNull(frequenciaBimestre2);
            Assert.Equal(alunoCodigo, frequenciaBimestre2.CodigoAluno);
            Assert.Equal(componenteCurricularId, frequenciaBimestre2.DisciplinaId);
            Assert.Equal(turmaCodigo, frequenciaBimestre2.TurmaId);
            Assert.Equal(25, frequenciaBimestre2.TotalAulas); 
            Assert.Equal(0, frequenciaBimestre2.TotalAusencias); 
            Assert.Equal(0, frequenciaBimestre2.TotalCompensacoes); 

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarSomenteAulasDadasQuandoNaoHaFrequenciasExistentes()
        {
            // Arrange
            var turmaCodigo = "789GHI";
            var alunoCodigo = "101JKL";
            var bimestres = new int[] { 1 };
            var componenteCurricularId = "POR";

            var dto = new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(turmaCodigo, alunoCodigo, bimestres, componenteCurricularId);

            var turmaMock = new Turma { CodigoTurma = turmaCodigo, Id = 20L };
            long tipoCalendarioIdMock = 200L;

            IEnumerable<FrequenciaAluno> frequenciasAlunoVazio = Enumerable.Empty<FrequenciaAluno>();

            var aulasComponentesTurmasMock = new List<TurmaComponenteQntAulasDto>
            {
                new TurmaComponenteQntAulasDto
                {
                    TurmaCodigo = turmaCodigo,
                    ComponenteCurricularCodigo = componenteCurricularId,
                    Bimestre = 1,
                    AulasQuantidade = 30
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioIdMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciasAlunoVazio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulasComponentesTurmasMock);

            // Act
            var result = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);

            var frequencia = result.First();
            Assert.Equal(alunoCodigo, frequencia.CodigoAluno);
            Assert.Equal(componenteCurricularId, frequencia.DisciplinaId);
            Assert.Equal(turmaCodigo, frequencia.TurmaId);
            Assert.Equal(30, frequencia.TotalAulas);
            Assert.Equal(0, frequencia.TotalAusencias);
            Assert.Equal(0, frequencia.TotalCompensacoes); 

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarNegocioExceptionQuandoTurmaNaoEncontrada()
        {
            // Arrange
            var dto = new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto("TURMA_NAO_EXISTE", "ALU123", new int[] { 1 }, "MAT");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(dto));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<NegocioException>(ex);
            Assert.Contains("Turma não encontrada!", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarNegocioExceptionQuandoTipoCalendarioNaoEncontrado()
        {
            // Arrange
            var turmaCodigo = "TURMA001";
            var dto = new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(turmaCodigo, "ALU123", new int[] { 1 }, "MAT");
            var turmaMock = new Turma { CodigoTurma = turmaCodigo, Id = 10L };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0L);

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(dto));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<NegocioException>(ex);
            Assert.Contains("Tipo calendário da turma não encontrada!", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarListaVaziaQuandoNaoHaFrequenciasOuAulas()
        {
            // Arrange
            var turmaCodigo = "EMPTY_TURMA";
            var alunoCodigo = "ALU_EMPTY";
            var bimestres = new int[] { 1 };
            var componenteCurricularId = "FIS";

            var dto = new FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto(turmaCodigo, alunoCodigo, bimestres, componenteCurricularId);

            var turmaMock = new Turma { CodigoTurma = turmaCodigo, Id = 30L };
            long tipoCalendarioIdMock = 300L;

            IEnumerable<FrequenciaAluno> frequenciasAlunoVazio = Enumerable.Empty<FrequenciaAluno>();
            IEnumerable<TurmaComponenteQntAulasDto> aulasComponentesTurmasVazio = Enumerable.Empty<TurmaComponenteQntAulasDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendarioIdMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequenciasAlunoVazio);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulasComponentesTurmasVazio);

            // Act
            var result = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}