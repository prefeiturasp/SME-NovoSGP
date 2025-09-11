using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.GoogleClassroom
{
    public class ExecutaSyncGsaGoogleClassroomUseCaseTeste
    {

        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IOptions<GoogleClassroomSyncOptions>> _optionsMock;

        public ExecutaSyncGsaGoogleClassroomUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _optionsMock = new Mock<IOptions<GoogleClassroomSyncOptions>>();
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_SeExecutarGsaSyncForFalse()
        {
            _optionsMock.Setup(o => o.Value).Returns(new GoogleClassroomSyncOptions
            {
                ExecutarGsaSync = false
            });

            var useCase = new ExecutaSyncGsaGoogleClassroomUseCase(_mediatorMock.Object, _optionsMock.Object);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.False(resultado);
            _mediatorMock.Verify(x => x.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveChamarMediator_SeExecutarGsaSyncForTrue()
        {
            _optionsMock.Setup(o => o.Value).Returns(new GoogleClassroomSyncOptions
            {
                ExecutarGsaSync = true
            });

            _mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var useCase = new ExecutaSyncGsaGoogleClassroomUseCase(_mediatorMock.Object, _optionsMock.Object);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaGoogleClassroomCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

