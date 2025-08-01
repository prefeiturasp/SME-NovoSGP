using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class PeriodoDeCompensacaoAbertoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PeriodoDeCompensacaoAbertoUseCase _useCase;

        public PeriodoDeCompensacaoAbertoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PeriodoDeCompensacaoAbertoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task VerificarPeriodoAberto_DeveRetornarTrue_QuandoParametroAtivoEAnoLetivoAtual()
        {
            // Arrange
            var turmaCodigo = "TURMA1";
            var bimestre = 2;
            var anoAtual = DateTime.Now.Year;

            var turma = new Turma { Id = 1, AnoLetivo = anoAtual };
            var parametro = new ParametrosSistema { Ativo = true };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametro);

            // Act
            var resultado = await _useCase.VerificarPeriodoAberto(turmaCodigo, bimestre);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task VerificarPeriodoAberto_DeveRetornarTrue_QuandoParametroAtivoETurmaEmPeriodoAberto()
        {
            // Arrange
            var turmaCodigo = "TURMA2";
            var bimestre = 1;
            var anoPassado = DateTime.Now.Year - 1;

            var turma = new Turma { AnoLetivo = anoPassado };
            var parametro = new ParametrosSistema { Ativo = true };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametro);

            _mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.VerificarPeriodoAberto(turmaCodigo, bimestre);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task VerificarPeriodoAberto_DeveRetornarFalse_QuandoParametroInativo()
        {
            // Arrange
            var turmaCodigo = "TURMA3";
            var bimestre = 3;
            var anoAtual = DateTime.Now.Year;

            var turma = new Turma { AnoLetivo = anoAtual };
            var parametro = new ParametrosSistema { Ativo = false };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametro);

            // Act
            var resultado = await _useCase.VerificarPeriodoAberto(turmaCodigo, bimestre);

            // Assert
            Assert.False(resultado);
        }

    }
}
