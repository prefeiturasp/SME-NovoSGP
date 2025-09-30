using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardInformacoesEscolares
{
    public class ObterDataConsolidacaoInformacoesEscolaresUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDataConsolidacaoInformacoesEscolaresUseCase _useCase;

        public ObterDataConsolidacaoInformacoesEscolaresUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDataConsolidacaoInformacoesEscolaresUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Nao_Encontrado_Deve_Lancar_NegocioException()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ParametrosSistema)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(2025));
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Com_Valor_Valido_Deve_Retornar_DateTime()
        {
            var dataString = "2025-09-30T14:16:00";
            var dataEsperada = DateTime.Parse(dataString);
            var parametroRetorno = new ParametrosSistema { Valor = dataString };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroRetorno);

            var resultado = await _useCase.Executar(2025);

            Assert.NotNull(resultado);
            Assert.Equal(dataEsperada, resultado);
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
        }
    }
}
