using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ObterFrequenciaDiariaAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFrequenciaDiariaAlunoUseCase _useCase;

        public ObterFrequenciaDiariaAlunoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterFrequenciaDiariaAlunoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarQueryCorretamenteERetornarDados()
        {
            // Arrange
            long turmaId = 1;
            long componenteCurricularId = 2;
            long alunoCodigo = 3;
            int bimestre = 1;
            int? semestre = 0;

            var filtroDto = new FiltroFrequenciaDiariaAlunoDto(turmaId, componenteCurricularId, alunoCodigo, bimestre, semestre);

            var frequenciasEsperadas = new List<FrequenciaDiariaAlunoDto>
            {
                new FrequenciaDiariaAlunoDto
                {
                    Id = 10,
                    DataAula = new DateTime(2025, 7, 1),
                    QuantidadeAulas = 2,
                    QuantidadePresenca = 2,
                    QuantidadeRemoto = 0,
                    QuantidadeAusencia = 0,
                    Motivo = null
                },
                new FrequenciaDiariaAlunoDto
                {
                    Id = 11,
                    DataAula = new DateTime(2025, 7, 2),
                    QuantidadeAulas = 2,
                    QuantidadePresenca = 1,
                    QuantidadeRemoto = 0,
                    QuantidadeAusencia = 1,
                    Motivo = "Doença"
                }
            };

            var paginacaoResultadoEsperado = new PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>
            {
                Items = frequenciasEsperadas,
                TotalPaginas = 1,
                TotalRegistros = frequenciasEsperadas.Count
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciaDiariaAlunoQuery>(q =>
                q.TurmaId == turmaId &&
                q.ComponenteCurricularId == componenteCurricularId &&
                q.AlunoCodigo == alunoCodigo &&
                q.Bimestre == bimestre &&
                q.Semestre == semestre
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginacaoResultadoEsperado);

            // Act
            var result = await _useCase.Executar(filtroDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paginacaoResultadoEsperado.TotalPaginas, result.TotalPaginas);
            Assert.Equal(paginacaoResultadoEsperado.TotalRegistros, result.TotalRegistros);
            Assert.Equal(paginacaoResultadoEsperado.Items.Count(), result.Items.Count());

            var primeiroItemEsperado = paginacaoResultadoEsperado.Items.First();
            var primeiroItemResultado = result.Items.First();

            Assert.Equal(primeiroItemEsperado.Id, primeiroItemResultado.Id);
            Assert.Equal(primeiroItemEsperado.DataAula, primeiroItemResultado.DataAula);
            Assert.Equal(primeiroItemEsperado.QuantidadeAulas, primeiroItemResultado.QuantidadeAulas);
            Assert.Equal(primeiroItemEsperado.QuantidadePresenca, primeiroItemResultado.QuantidadePresenca);
            Assert.Equal(primeiroItemEsperado.QuantidadeRemoto, primeiroItemResultado.QuantidadeRemoto);
            Assert.Equal(primeiroItemEsperado.QuantidadeAusencia, primeiroItemResultado.QuantidadeAusencia);
            Assert.Equal(primeiroItemEsperado.Motivo, primeiroItemResultado.Motivo);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaDiariaAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarVazioQuandoQueryNaoEncontrarResultados()
        {
            // Arrange
            long turmaId = 4;
            long componenteCurricularId = 5;
            long alunoCodigo = 6;
            int bimestre = 2;
            int? semestre = 0;

            var filtroDto = new FiltroFrequenciaDiariaAlunoDto(turmaId, componenteCurricularId, alunoCodigo, bimestre, semestre);

            var paginacaoResultadoVazio = new PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>
            {
                Items = Enumerable.Empty<FrequenciaDiariaAlunoDto>(),
                TotalPaginas = 0,
                TotalRegistros = 0
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaDiariaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginacaoResultadoVazio);

            // Act
            var result = await _useCase.Executar(filtroDto);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalPaginas);
            Assert.Equal(0, result.TotalRegistros);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaDiariaAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoSeMediatorFalhar()
        {
            // Arrange
            long turmaId = 7;
            long componenteCurricularId = 8;
            long alunoCodigo = 9;
            int bimestre = 3;
            int? semestre = 0;

            var filtroDto = new FiltroFrequenciaDiariaAlunoDto(turmaId, componenteCurricularId, alunoCodigo, bimestre, semestre);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaDiariaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro na consulta de frequência diária"));

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(filtroDto));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<System.Exception>(ex);
            Assert.Contains("Erro na consulta de frequência diária", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaDiariaAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}