using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoAlfabetizacaoCriticaEscrita;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Sondagem
{
    public class ObterConsolidacaoAlfabetizacaoCriticaEscritaQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterConsolidacaoAlfabetizacaoCriticaEscritaQueryHandler _sut;
        private readonly Faker<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto> _dtoFaker;

        public ObterConsolidacaoAlfabetizacaoCriticaEscritaQueryHandlerTeste()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _mediatorMock = new Mock<IMediator>();
            _sut = new ObterConsolidacaoAlfabetizacaoCriticaEscritaQueryHandler(_httpClientFactoryMock.Object, _mediatorMock.Object);

            _dtoFaker = new Faker<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>("pt_BR")
                .RuleFor(s => s.DreCodigo, f => f.Random.AlphaNumeric(10).ToUpper())
                .RuleFor(s => s.UeCodigo, f => f.Random.AlphaNumeric(10).ToUpper())
                .RuleFor(s => s.QuantidadeNaoAlfabetizados, f => f.Random.Int(1, 50))
                .RuleFor(s => s.PercentualNaoAlfabetizados, f => f.Random.Decimal(1, 100));
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
        public async Task Handle_QuandoRequisicaoSucedida_DeveRetornarConsolidacao()
        {
            // Arrange
            var listaEsperada = _dtoFaker.Generate(5);
            var jsonRetorno = JsonConvert.SerializeObject(listaEsperada);
            ConfigurarMockHttpClient(HttpStatusCode.OK, jsonRetorno);

            var query = new ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery();

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
            var query = new ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery();

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
            var query = new ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery();

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _sut.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
