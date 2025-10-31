using Bogus;
using MediatR;
using Moq;
using Moq.Protected;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Professores.ObterProfessorPorRFUeDreAnoLetivo
{
    public class ObterProfessorPorRFUeDreAnoLetivoQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> httpClientFactory;
        private readonly Mock<IMediator> mediator;
        private readonly ObterProfessorPorRFUeDreAnoLetivoQueryHandler queryHandler;
        private readonly Faker faker;

        public ObterProfessorPorRFUeDreAnoLetivoQueryHandlerTeste()
        {
            httpClientFactory = new Mock<IHttpClientFactory>();
            mediator = new Mock<IMediator>();
            queryHandler = new ObterProfessorPorRFUeDreAnoLetivoQueryHandler(httpClientFactory.Object, mediator.Object);
            faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "ObterProfessorPorRFUeDreAnoLetivo - Deve retornar dados mesmo sem atribuições ativas")]
        public async Task DeveRetornarDadosMesmoSemAtribuicoesAtivas()
        {
            // Arrange
            var anoLetivo = DateTime.Now.Year;
            var codigoRf = faker.Random.Replace("########");
            var ueCodigo = faker.Random.AlphaNumeric(6);
            var dreCodigo = faker.Random.AlphaNumeric(6);

            // 1. Simula a resposta do EOL como "sem conteúdo"
            ConfigurarMockHttpClient(HttpStatusCode.NoContent);

            // 2. Cria um usuário que será retornado pelo banco de dados do SGP
            var usuario = new Faker<Usuario>()
                .RuleFor(u => u.Id, faker.Random.Long(1, 100))
                .RuleFor(u => u.CodigoRf, codigoRf)
                .RuleFor(u => u.Nome, f => f.Person.FullName)
                .Generate();

            usuario.DefinirPerfis(new List<PrioridadePerfil> { new() { CodigoPerfil = Perfis.PERFIL_PROFESSOR } });

            mediator.Setup(m => m.Send(It.Is<ObterUsuariosPorRfOuCriaQuery>(q => q.CodigosRf.Contains(codigoRf)), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Usuario> { usuario });

            mediator.Setup(m => m.Send(It.Is<ObterAtribuicoesCJAtivasQuery>(q => q.CodigoRf == codigoRf), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Enumerable.Empty<AtribuicaoCJ>());

            var query = new ObterProfessorPorRFUeDreAnoLetivoQuery(codigoRf, anoLetivo, dreCodigo, ueCodigo, true, false);

            // Act
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuario.CodigoRf, resultado.CodigoRF);
            Assert.Equal(usuario.Nome, resultado.Nome);
            Assert.Equal(usuario.Id, resultado.UsuarioId);
        }

        private void ConfigurarMockHttpClient(HttpStatusCode statusCode, string responseContent = "")
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseContent)
                });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://localhost/") };
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        }
    }
}
