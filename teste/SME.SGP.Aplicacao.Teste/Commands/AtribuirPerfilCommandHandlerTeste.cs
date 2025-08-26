using MediatR;
using Moq;
using Moq.Protected;
using SME.SGP.Dominio;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands
{
    public class AtribuirPerfilCommandHandlerTeste
    {
        private readonly Guid perfilTeste = Guid.NewGuid();
        private readonly string codigoRfTeste = "123456";

        private HttpClient CriarHttpClientFake(HttpStatusCode statusCode, string conteudoRetorno = "")
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(conteudoRetorno)
                });

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost") 
            };

            return client;
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Unit_Quando_Resposta_For_Sucesso()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>()))
                                 .Returns(CriarHttpClientFake(HttpStatusCode.OK));

            var handler = new AtribuirPerfilCommandHandler(httpClientFactoryMock.Object);
            var command = new AtribuirPerfilCommand(codigoRfTeste, perfilTeste);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, resultado);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_Negocio_Exception_Quando_Resposta_For_Falha()
        {
            var mensagemErro = "Erro ao atribuir perfil";
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>()))
                                 .Returns(CriarHttpClientFake(HttpStatusCode.BadRequest, mensagemErro));

            var handler = new AtribuirPerfilCommandHandler(httpClientFactoryMock.Object);
            var command = new AtribuirPerfilCommand(codigoRfTeste, perfilTeste);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(mensagemErro, exception.Message);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Argument_Null_Exception_Quando_Http_Client_Factory_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new AtribuirPerfilCommandHandler(null));
        }
    }
}
