using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ObterFaltasNaoCompensadaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFaltasNaoCompensadaUseCase _useCase;

        public ObterFaltasNaoCompensadaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterFaltasNaoCompensadaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarQueryCorretamenteERetornarDados()
        {
            // Arrange
            var compensacaoId = 1;
            var turmaId = "123T";
            var disciplinaId = "MAT";
            var codigoAluno = "ALU001";
            var quantidadeCompensar = 2;
            var bimestre = 1;

            var filtro = new FiltroFaltasNaoCompensadasDto
            {
                CompensacaoId = compensacaoId,
                TurmaId = turmaId,
                DisciplinaId = disciplinaId,
                CodigoAluno = codigoAluno,
                QuantidadeCompensar = quantidadeCompensar,
                Bimestre = bimestre
            };

            var resultadosEsperados = new List<RegistroFaltasNaoCompensadaDto>
            {
                new RegistroFaltasNaoCompensadaDto
                {
                    CodigoAluno = codigoAluno,
                    AulaId = 101,
                    DataAula = new System.DateTime(2025, 3, 10),
                    NumeroAula = 1,
                    Descricao = "Aula de Matemática",
                    RegistroFrequenciaAlunoId = 201,
                    Sugestao = false
                }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAusenciaParaCompensacaoQuery>(query =>
                query.CompensacaoId == compensacaoId &&
                query.TurmaCodigo == turmaId &&
                query.DisciplinaId == disciplinaId &&
                query.Bimestre == bimestre &&
                query.AlunosQuantidadeCompensacoes.Count() == 1 &&
                query.AlunosQuantidadeCompensacoes.First().CodigoAluno == codigoAluno &&
                query.AlunosQuantidadeCompensacoes.First().QuantidadeCompensar == quantidadeCompensar
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultadosEsperados);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(resultadosEsperados.Count, result.Count());
            Assert.Equal(resultadosEsperados.First().CodigoAluno, result.First().CodigoAluno);
            Assert.Equal(resultadosEsperados.First().AulaId, result.First().AulaId);
            Assert.Equal(resultadosEsperados.First().DataAula, result.First().DataAula);
            Assert.Equal(resultadosEsperados.First().NumeroAula, result.First().NumeroAula);
            Assert.Equal(resultadosEsperados.First().Descricao, result.First().Descricao);
            Assert.Equal(resultadosEsperados.First().RegistroFrequenciaAlunoId, result.First().RegistroFrequenciaAlunoId);
            Assert.Equal(resultadosEsperados.First().Sugestao, result.First().Sugestao);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarVazioQuandoNaoEncontrarResultados()
        {
            // Arrange
            var filtro = new FiltroFaltasNaoCompensadasDto
            {
                CompensacaoId = 1,
                TurmaId = "456T",
                DisciplinaId = "HIS",
                CodigoAluno = "ALU002",
                QuantidadeCompensar = 1,
                Bimestre = 2
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<RegistroFaltasNaoCompensadaDto>());

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoSeMediatorFalhar()
        {
            // Arrange
            var filtro = new FiltroFaltasNaoCompensadasDto
            {
                CompensacaoId = 1,
                TurmaId = "789T",
                DisciplinaId = "GEO",
                CodigoAluno = "ALU003",
                QuantidadeCompensar = 3,
                Bimestre = 3
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro na consulta do MediatR"));

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(filtro));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<System.Exception>(ex);
            Assert.Contains("Erro na consulta do MediatR", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}