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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PeriodosEscolares
{
    public class ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase _useCase;

        public ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_DeveLancarArgumentNullExceptionQuandoMediatorNulo()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase(null));
            Assert.Contains("mediator", ex.Message);
        }

        [Fact]
        public async Task Executar_DeveRetornarListaDePeriodoEscolarDtoComSucesso()
        {
            // Arrange
            var modalidade = Modalidade.Fundamental;
            var anoLetivo = 2025;
            var semestre = 0;

            var periodosEscolaresMock = new List<PeriodoEscolar>
            {
                new PeriodoEscolar { Id = 1, Bimestre = 1, Migrado = false, PeriodoInicio = new DateTime(2025, 2, 1), PeriodoFim = new DateTime(2025, 4, 30) },
                new PeriodoEscolar { Id = 2, Bimestre = 2, Migrado = false, PeriodoInicio = new DateTime(2025, 5, 1), PeriodoFim = new DateTime(2025, 7, 31) }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(q =>
                q.ModalidadeTurma == modalidade && q.AnoLetivo == anoLetivo && q.Semestre == semestre
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodosEscolaresMock);

            // Act
            var result = await _useCase.Executar(modalidade, anoLetivo, semestre);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(periodosEscolaresMock.Count, result.Count());

            for (int i = 0; i < periodosEscolaresMock.Count; i++)
            {
                var expected = periodosEscolaresMock.ElementAt(i);
                var actual = result.ElementAt(i);

                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Bimestre, actual.Bimestre);
                Assert.Equal(expected.Migrado, actual.Migrado);
                Assert.Equal(expected.PeriodoInicio, actual.PeriodoInicio);
                Assert.Equal(expected.PeriodoFim, actual.PeriodoFim);
                Assert.Equal($"{expected.Bimestre}° Bimestre", actual.Descricao);
            }

            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(q =>
                q.ModalidadeTurma == modalidade && q.AnoLetivo == anoLetivo && q.Semestre == semestre
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarListaVaziaQuandoNenhumPeriodoEncontrado()
        {
            // Arrange
            var modalidade = Modalidade.EJA;
            var anoLetivo = 2024;
            var semestre = 1;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(q =>
                q.ModalidadeTurma == modalidade && q.AnoLetivo == anoLetivo && q.Semestre == semestre
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PeriodoEscolar>());

            // Act
            var result = await _useCase.Executar(modalidade, anoLetivo, semestre);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(q =>
                q.ModalidadeTurma == modalidade && q.AnoLetivo == anoLetivo && q.Semestre == semestre
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarNullQuandoQueryRetornaNull()
        {
            // Arrange
            var modalidade = Modalidade.Fundamental;
            var anoLetivo = 2025;
            var semestre = 0;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<PeriodoEscolar>)null); 

            // Act
            var result = await _useCase.Executar(modalidade, anoLetivo, semestre);

            // Assert
            Assert.Null(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoQueryFalhar()
        {
            // Arrange
            var modalidade = Modalidade.EducacaoInfantil;
            var anoLetivo = 2023;
            var semestre = 0;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao buscar períodos escolares na base de dados"));

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(modalidade, anoLetivo, semestre));

            // Assert
            Assert.Contains("Erro ao buscar períodos escolares na base de dados", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void PeriodoEscolarDto_Descricao_DeveRetornarBimestreCorreto()
        {
            // Arrange
            var dto1 = new PeriodoEscolarDto { Bimestre = 1 };
            var dto2 = new PeriodoEscolarDto { Bimestre = 4 };
            var dto3 = new PeriodoEscolarDto { Bimestre = 0 };

            // Act & Assert
            Assert.Equal("1° Bimestre", dto1.Descricao);
            Assert.Equal("4° Bimestre", dto2.Descricao);
            Assert.Equal("", dto3.Descricao);
        }

        [Fact]
        public void PeriodoEscolarDto_MesesDoPeriodo_DeveRetornarMesesCorretos()
        {
            // Arrange
            var dto = new PeriodoEscolarDto
            {
                PeriodoInicio = new DateTime(2025, 3, 15),
                PeriodoFim = new DateTime(2025, 5, 20)
            };

            // Act
            var meses = dto.MesesDoPeriodo().ToList();

            // Assert
            Assert.Equal(3, meses.Count);
            Assert.Contains(3, meses);
            Assert.Contains(4, meses);
            Assert.Contains(5, meses);
            Assert.DoesNotContain(2, meses);
            Assert.DoesNotContain(6, meses);
        }
    }
}