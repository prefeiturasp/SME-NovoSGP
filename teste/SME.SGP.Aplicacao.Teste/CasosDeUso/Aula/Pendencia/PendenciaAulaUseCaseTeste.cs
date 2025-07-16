using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.Pendencia
{
    public class PendenciaAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PendenciaAulaUseCase _useCase;

        public PendenciaAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PendenciaAulaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DevePublicarComando_ParaCadaDre_QuandoDataValida()
        {
            // Arrange
            var dataHoje = DateTimeExtension.HorarioBrasilia().Date;
            var parametro = new ParametrosSistema { Valor = dataHoje.AddDays(-1).ToString("yyyy-MM-dd") };

            var idsDres = new List<long> { 10, 20 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametro);

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(idsDres);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var mensagem = new MensagemRabbit(""); // conteúdo irrelevante para esse caso

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoDataInvalida()
        {
            // Arrange: data futura, ainda não atingida
            var parametro = new ParametrosSistema { Valor = DateTimeExtension.HorarioBrasilia().AddDays(2).ToString("yyyy-MM-dd") };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametro);

            var mensagem = new MensagemRabbit(""); // conteúdo irrelevante

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);

            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoParametroNulo()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ParametrosSistema)null);

            var mensagem = new MensagemRabbit(""); // conteúdo irrelevante

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
        }
    }
}
