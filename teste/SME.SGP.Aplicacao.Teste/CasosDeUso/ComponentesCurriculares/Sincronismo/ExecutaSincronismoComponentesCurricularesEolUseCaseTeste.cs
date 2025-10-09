using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ComponentesCurriculares.Sincronismo
{
    public class ExecutaSincronismoComponentesCurricularesEolUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutaSincronismoComponentesCurricularesEolUseCase _useCase;

        public ExecutaSincronismoComponentesCurricularesEolUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutaSincronismoComponentesCurricularesEolUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Publicar_Comando_Na_Fila_Correta()
        {
            var mensagemRabbit = new MensagemRabbit();

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
             It.Is<PublicarFilaSgpCommand>(c =>
                 c.Rota == RotasRabbitSgp.RotaSincronizaComponetesCurricularesEol &&
                 c.Filtros is ExecutarSincronismoComponentesCurricularesUseCase),
             It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
