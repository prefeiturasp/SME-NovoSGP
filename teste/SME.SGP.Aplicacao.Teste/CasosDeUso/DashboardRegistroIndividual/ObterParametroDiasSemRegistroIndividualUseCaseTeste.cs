using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardRegistroIndividual
{
    public class ObterParametroDiasSemRegistroIndividualUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterParametroDiasSemRegistroIndividualUseCase _useCase;

        public ObterParametroDiasSemRegistroIndividualUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterParametroDiasSemRegistroIndividualUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Nao_Encontrado_Deve_Lancar_NegocioException()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ParametrosSistema)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(2025));
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Com_Valor_Valido_Deve_Retornar_Int()
        {
            var valorEsperado = 15;
            var parametroRetorno = new ParametrosSistema { Valor = valorEsperado.ToString() };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroRetorno);

            var resultado = await _useCase.Executar(2025);

            Assert.NotNull(resultado);
            Assert.Equal(valorEsperado, resultado);
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
