using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardRegistroIndividual
{
    public class ObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase _useCase;

        public ObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Com_Valor_Valido_Deve_Retornar_DateTime()
        {
            var dataString = "2025-09-30T14:50:00";
            var dataEsperada = DateTime.Parse(dataString);
            var parametroRetorno = new ParametrosSistema { Valor = dataString };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroRetorno);

            var resultado = await _useCase.Executar(2025);

            Assert.NotNull(resultado);
            Assert.Equal(dataEsperada, resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Executar_Quando_Parametro_Com_Valor_Invalido_Deve_Retornar_Null(string valorInvalido)
        {
            var parametroRetorno = new ParametrosSistema { Valor = valorInvalido };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroRetorno);

            var resultado = await _useCase.Executar(2025);

            Assert.Null(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
