using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Estudante
{
    public class ExcluirEstudanteFotoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ExcluirEstudanteFotoUseCase useCase;

        public ExcluirEstudanteFotoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ExcluirEstudanteFotoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_RetornarTrue_Teste()
        {
            var codigoAluno = "12345";
            mediatorMock.Setup(m => m.Send(It.Is<ExcluirFotoEstudanteCommand>(c => c.AlunoCodigo == codigoAluno), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(codigoAluno);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ExcluirFotoEstudanteCommand>(c => c.AlunoCodigo == codigoAluno), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_ComCodigoNulo_Deve_RetornarFalse_Teste()
        {
            string codigoAluno = null;
            mediatorMock.Setup(m => m.Send(It.Is<ExcluirFotoEstudanteCommand>(c => c.AlunoCodigo == codigoAluno), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            var resultado = await useCase.Executar(codigoAluno);

            Assert.False(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ExcluirFotoEstudanteCommand>(c => c.AlunoCodigo == codigoAluno), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
