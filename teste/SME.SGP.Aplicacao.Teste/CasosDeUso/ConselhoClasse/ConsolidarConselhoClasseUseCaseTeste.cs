using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ConsolidarConselhoClasseUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarConselhoClasseUseCase _useCase;

        public ConsolidarConselhoClasseUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarConselhoClasseUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Comando_De_Consolidacao_De_Conselho_Com_Sucesso()
        {
            int dreId = 123;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsolidarConselhoClasseCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(dreId);

            Assert.True(resultado);

            _mediatorMock.Verify(m =>
                m.Send(It.Is<ConsolidarConselhoClasseCommand>(c => c.DreId == dreId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
