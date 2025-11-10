using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.Dre
{
    public class ExecutarSincronizacaoInstitucionalDreTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalDreTratarUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalDreTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalDreTratarUseCase(_mediatorMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Executar_Quando_Codigo_Dre_Nulo_Ou_Vazio_Deve_Lancar_Negocio_Exception(string codigoDre)
        {
            var mensagemRabbit = new MensagemRabbit { Mensagem = codigoDre };

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar o código da Dre.", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Codigo_Dre_Valido_Deve_Enviar_Comando_E_Retornar_Verdadeiro()
        {
            var dreCodigo = "987654321";
            var mensagemRabbit = new MensagemRabbit { Mensagem = dreCodigo };

            _mediatorMock.Setup(m => m.Send(It.IsAny<TrataSincronizacaoInstitucionalDreCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<TrataSincronizacaoInstitucionalDreCommand>(c => c.DreCodigo == long.Parse(dreCodigo)),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
