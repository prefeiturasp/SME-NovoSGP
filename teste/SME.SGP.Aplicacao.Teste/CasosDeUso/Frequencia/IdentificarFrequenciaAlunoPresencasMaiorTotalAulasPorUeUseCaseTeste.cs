using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase _useCase;

        public IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DevePublicarMensagensQuandoRegistrosIrregularesEncontrados()
        {
            // Arrange
            long ueId = 123;
            int anoLetivo = 2025;
            Guid correlationId = Guid.NewGuid();

            var filtro = new FiltroIdentificarFrequenciaAlunoPresencasMaiorQuantidadeAulasDto
            {
                UeId = ueId,
                AnoLetivo = anoLetivo
            };
            var mensagemRabbit = new MensagemRabbit("AcaoTeste", JsonConvert.SerializeObject(filtro), correlationId, "usuarioRF");

            long[] registrosIrregulares = { 1, 2, 3 };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery>(q =>
                q.UeId == ueId && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registrosIrregulares);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(result);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery>(q =>
                q.UeId == ueId && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpFrequencia.RegularizarFrequenciaAlunoPresencasMaiorTotalAulas &&
                (long[])cmd.Filtros == registrosIrregulares &&
                cmd.CodigoCorrelacao == correlationId), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_NaoDevePublicarMensagensQuandoNenhumRegistroIrregularEncontrado()
        {
            // Arrange
            long ueId = 456;
            int anoLetivo = 2024;
            Guid correlationId = Guid.NewGuid();

            var filtro = new FiltroIdentificarFrequenciaAlunoPresencasMaiorQuantidadeAulasDto
            {
                UeId = ueId,
                AnoLetivo = anoLetivo
            };
            var mensagemRabbit = new MensagemRabbit("AcaoTeste", JsonConvert.SerializeObject(filtro), correlationId, "usuarioRF");

            long[] registrosIrregularesVazios = Array.Empty<long>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registrosIrregularesVazios);

            // Act
            var result = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(result);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery>(q =>
                q.UeId == ueId && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoFiltroInvalido()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit("AcaoTeste", null, Guid.NewGuid(), "usuarioRF");

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(mensagemRabbit));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<NullReferenceException>(ex);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoConsultaDeRegistrosFalhar()
        {
            // Arrange
            long ueId = 789;
            int anoLetivo = 2023;
            Guid correlationId = Guid.NewGuid();

            var filtro = new FiltroIdentificarFrequenciaAlunoPresencasMaiorQuantidadeAulasDto
            {
                UeId = ueId,
                AnoLetivo = anoLetivo
            };
            var mensagemRabbit = new MensagemRabbit("AcaoTeste", JsonConvert.SerializeObject(filtro), correlationId, "usuarioRF");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter registros irregulares"));

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(mensagemRabbit));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            Assert.Contains("Erro ao obter registros irregulares", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery>(q =>
                q.UeId == ueId && q.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}