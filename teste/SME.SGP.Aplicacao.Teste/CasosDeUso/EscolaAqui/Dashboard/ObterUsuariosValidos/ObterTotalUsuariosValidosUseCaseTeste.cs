using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterUsuariosValidos
{
    public class ObterTotalUsuariosValidosUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterTotalUsuariosValidosUseCase useCase;

        public ObterTotalUsuariosValidosUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterTotalUsuariosValidosUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Total_Usuarios_Validos()
        {
            string codigoDre = "01";
            long codigoUe = 123;
            long totalEsperado = 42;

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTotalUsuariosValidosQuery>(q =>
                    q.CodigoDre == codigoDre && q.CodigoUe == codigoUe),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalEsperado);

            var resultado = await useCase.Executar(codigoDre, codigoUe);

            Assert.Equal(totalEsperado, resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTotalUsuariosValidosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Zero_Quando_Mediator_Retorna_Zero()
        {
            string codigoDre = "00";
            long codigoUe = 0;
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTotalUsuariosValidosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            var resultado = await useCase.Executar(codigoDre, codigoUe);

            Assert.Equal(0, resultado);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Mediator_For_Nulo()
        {
            Assert.Throws<System.ArgumentNullException>(() =>
                new ObterTotalUsuariosValidosUseCase(null));
        }
    }
}
