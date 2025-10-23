using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEEObservacao
{
    public class ExcluirPlanoAEEObservacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirPlanoAEEObservacaoUseCase _useCase;

        public ExcluirPlanoAEEObservacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirPlanoAEEObservacaoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Id_Valido_Deve_Enviar_Comando_E_Retornar_Resultado_()
        {
            long id = 123;

            _mediatorMock.Setup(m => m.Send(It.Is<ExcluirPlanoAEEObservacaoCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(id);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirPlanoAEEObservacaoCommand>(c => c.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
