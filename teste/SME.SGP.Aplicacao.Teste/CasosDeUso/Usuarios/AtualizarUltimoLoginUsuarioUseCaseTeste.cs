using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Usuarios
{
    public class AtualizarUltimoLoginUsuarioUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly AtualizarUltimoLoginUsuarioUseCase useCase;

        public AtualizarUltimoLoginUsuarioUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new AtualizarUltimoLoginUsuarioUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Teste_Teste()
        {
            var usuario = new Usuario { CodigoRf = "123" };
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(usuario));

            mediatorMock.Setup(x => x.Send(It.IsAny<AtualizarUltimoLoginUsuarioCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.Is<AtualizarUltimoLoginUsuarioCommand>(c => c.Usuario.CodigoRf == "123"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Nula_Deve_Teste_Teste()
        {
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject("null"));

            await Assert.ThrowsAsync<Newtonsoft.Json.JsonSerializationException>(() => useCase.Executar(mensagem));
        }
    }
}
