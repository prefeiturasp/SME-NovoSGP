using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class AlterarObservacaoDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly AlterarObservacaoDiarioBordoUseCase alterarObservacaoDiarioBordoUseCase;

        public AlterarObservacaoDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            alterarObservacaoDiarioBordoUseCase = new AlterarObservacaoDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Alterar_Observacao_Diario_De_Bordo()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<AlterarObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto()
                {
                    Id = 1
                });

            //Act
            var auditoriaDto = await alterarObservacaoDiarioBordoUseCase.Executar("observacao", 1);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<AlterarObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }
    }
}
