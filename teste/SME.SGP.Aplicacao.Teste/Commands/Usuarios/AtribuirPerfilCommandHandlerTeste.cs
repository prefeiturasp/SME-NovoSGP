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

namespace SME.SGP.Aplicacao.Teste.Commands.Usuarios
{
    public class AtribuirPerfilCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Atribuir_Perfil_Quando_Sucesso()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var handler = new AtribuirPerfilCommandHandler(httpClientFactoryMock.Object);
            var command = new AtribuirPerfilCommand("123456", Guid.NewGuid());

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Falha()
        {
            var mensagemErro = "Erro ao atribuir perfil";
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(mensagemErro)
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var handler = new AtribuirPerfilCommandHandler(httpClientFactoryMock.Object);
            var command = new AtribuirPerfilCommand("123456", Guid.NewGuid());

            var ex = await Assert.ThrowsAsync<NegocioException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal(mensagemErro, ex.Message);
        }
    }
}
