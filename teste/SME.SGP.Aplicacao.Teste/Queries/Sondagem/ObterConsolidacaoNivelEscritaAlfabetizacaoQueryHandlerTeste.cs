using MediatR;
using Moq.Protected;
using Moq;
using SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoNivelEscritaAlfabetizacao;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using Bogus;
using FluentAssertions;
using SME.SGP.Infra.Consts;
using Newtonsoft.Json;
using System;

namespace SME.SGP.Aplicacao.Teste.Queries.Sondagem
{
    public class ObterConsolidacaoNivelEscritaAlfabetizacaoQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterConsolidacaoNivelEscritaAlfabetizacaoQueryHandler _sut;

        private static readonly Faker<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto> _sondagemFaker =
            new Faker<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>("pt_BR")
                .RuleFor(s => s.DreCodigo, f => f.Random.Number(100000, 999999).ToString())
                .RuleFor(s => s.UeCodigo, f => f.Random.Number(100000, 999999).ToString())
                .RuleFor(s => s.AnoLetivo, f => f.Date.Recent().Year.ToString())
                .RuleFor(s => s.NivelEscrita, f => f.PickRandom(new[] { "A", "PS", "SA", "SCV", "SSV" }))
                .RuleFor(s => s.Periodo, f => f.Random.Short(1, 4))
                .RuleFor(s => s.QuantidadeAlunos, f => f.Random.Int(5, 35));

        public ObterConsolidacaoNivelEscritaAlfabetizacaoQueryHandlerTeste()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _mediatorMock = new Mock<IMediator>();

            _sut = new ObterConsolidacaoNivelEscritaAlfabetizacaoQueryHandler(_httpClientFactoryMock.Object, _mediatorMock.Object);
        }

        private void ConfigurarMockHttpClient(HttpStatusCode statusCode, string jsonContent)
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var resposta = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            httpMessageHandlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(resposta);

            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5001/")
            };
            _httpClientFactoryMock.Setup(f => f.CreateClient(ServicoSondagemConstants.Servico)).Returns(httpClient);
        }

        [Fact]
        public async Task Handle_QuandoRequisicaoForSucedida_DeveRetornarConsolidacao()
        {
            // Arrange
            var listaEsperada = _sondagemFaker.Generate(3);
            var jsonRetorno = JsonConvert.SerializeObject(listaEsperada);
            ConfigurarMockHttpClient(HttpStatusCode.OK, jsonRetorno);

            var query = new ObterConsolidacaoNivelEscritaAlfabetizacaoQuery();

            // Act
            var resultado = await _sut.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(listaEsperada);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_QuandoRequisicaoRetornarSemConteudo_DeveRetornarListaVazia()
        {
            // Arrange
            ConfigurarMockHttpClient(HttpStatusCode.NoContent, string.Empty);
            var query = new ObterConsolidacaoNivelEscritaAlfabetizacaoQuery();

            // Act
            var resultado = await _sut.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_QuandoRequisicaoFalhar_DeveLogarErroERetornarListaVazia()
        {
            // Arrange
            ConfigurarMockHttpClient(HttpStatusCode.InternalServerError, "Erro interno no servidor de sondagem.");
            var query = new ObterConsolidacaoNivelEscritaAlfabetizacaoQuery();

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var resultado = await _sut.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
