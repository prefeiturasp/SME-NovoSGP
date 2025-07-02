using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtribuicaoResponsaveis
{
    public class RemoverAtribuicaoResponsaveisUseCaseTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly RemoverAtribuicaoResponsaveisUseCase useCase;

        public RemoverAtribuicaoResponsaveisUseCaseTests()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new RemoverAtribuicaoResponsaveisUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Com_Sucesso_E_Publicar_Todas_As_Filas()
        {
            var codigosDres = new string[] { "dre1", "dre2" };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigosDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosDres);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = new MensagemRabbit { Mensagem = string.Empty };

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterCodigosDresQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

