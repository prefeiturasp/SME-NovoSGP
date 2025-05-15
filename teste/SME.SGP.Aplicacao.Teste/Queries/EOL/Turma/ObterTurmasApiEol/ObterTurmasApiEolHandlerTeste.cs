using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.EOL.Turma.ObterTurmasApiEol
{
    public class ObterTurmasApiEolHandlerTeste
    {
        [Fact(DisplayName = "Deve retornar turmas ao chamar a API EOL com sucesso")]
        public async Task DeveRetornarTurmasQuandoRespostaEhSucesso()
        {
            // Arrange
            var turmasEsperadas = new List<TurmaApiEolDto>
            {
                new TurmaApiEolDto { Codigo = 2839516, Extinta = false},
                new TurmaApiEolDto { Codigo = 2849122, Extinta = true},
                new TurmaApiEolDto { Codigo = 2877516, Extinta = false},
            };

            var respostaJson = JsonConvert.SerializeObject(turmasEsperadas);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(respostaJson)
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new System.Uri("https://api.eol.com.br/somefakeurl")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.Is<string>(s => s == ServicosEolConstants.SERVICO)))
                .Returns(httpClient);

            var handler = new ObterTurmasApiEolHandler(httpClientFactoryMock.Object);

            var query = new ObterTurmasApiEolQuery(new List<string> { "2839516", "2849122", "2877516" });

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.DoesNotContain(resultado, t => t.Extinta == true);
        }


        [Fact(DisplayName = "Deve retornar lista vazia quando resposta da API for NoContent")]
        public async Task DeveRetornarListaVaziaQuandoApiRetornarNoContent()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NoContent)
            {
                Content = new StringContent(string.Empty) // Corpo vazio simula o 204
            };

            var handlerStub = new HttpMessageHandlerStub(async (request, cancellationToken) =>
            {
                return await Task.FromResult(responseMessage);
            });

            var httpClient = new HttpClient(handlerStub)
            {
                BaseAddress = new Uri("https://fakeurl.api")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.Is<string>(s => s == ServicosEolConstants.SERVICO)))
                .Returns(httpClient);

            var handler = new ObterTurmasApiEolHandler(httpClientFactoryMock.Object);

            var query = new ObterTurmasApiEolQuery(new List<string> { "2839516", "2849122" });

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

    }
}
