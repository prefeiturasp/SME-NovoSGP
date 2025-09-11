using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterUsuariosComAcessoIncompleto
{
    public class ObterTotalUsuariosComAcessoIncompletoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterTotalUsuariosComAcessoIncompletoUseCase useCase;

        public ObterTotalUsuariosComAcessoIncompletoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterTotalUsuariosComAcessoIncompletoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Total_Retornado_Pelo_Mediator()
        {
            var codigoDre = "01";
            var codigoUe = 123L;
            var totalEsperado = 42L;

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTotalUsuariosComAcessoIncompletoQuery>(q =>
                    q.CodigoDre == codigoDre && q.CodigoUe == codigoUe),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalEsperado);

            var resultado = await useCase.Executar(codigoDre, codigoUe);

            Assert.Equal(totalEsperado, resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTotalUsuariosComAcessoIncompletoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            var codigoDre = "DRE123";
            var codigoUe = 456L;

            ObterTotalUsuariosComAcessoIncompletoQuery? queryEnviada = null;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTotalUsuariosComAcessoIncompletoQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<long>, CancellationToken>((req, _) => queryEnviada = (ObterTotalUsuariosComAcessoIncompletoQuery)req)
                .ReturnsAsync(10L);

            await useCase.Executar(codigoDre, codigoUe);

            Assert.NotNull(queryEnviada);
            Assert.Equal(codigoDre, queryEnviada.CodigoDre);
            Assert.Equal(codigoUe, queryEnviada.CodigoUe);
        }

        [Fact]
        public void Construtor_Deve_Lancar_ArgumentNullException_Se_Mediator_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTotalUsuariosComAcessoIncompletoUseCase(null!));
        }
    }
}
