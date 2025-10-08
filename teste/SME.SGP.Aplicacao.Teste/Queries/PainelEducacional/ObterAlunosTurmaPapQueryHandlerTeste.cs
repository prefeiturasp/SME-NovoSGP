using Bogus;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterAlunosTurmaPapQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly ObterAlunosTurmaPapQueryHandler _handler;
        private readonly Faker<DadosMatriculaAlunoTipoPapDto> _alunoFaker;

        public ObterAlunosTurmaPapQueryHandlerTeste()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://teste.com/")
            };

            _httpClientFactoryMock.Setup(x => x.CreateClient(ServicosEolConstants.SERVICO))
                .Returns(httpClient);
            _handler = new ObterAlunosTurmaPapQueryHandler(_httpClientFactoryMock.Object);

            _alunoFaker = new Faker<DadosMatriculaAlunoTipoPapDto>("pt_BR")
                .RuleFor(a => a.AnoLetivo, f => f.Random.Int(2020, 2025))
                .RuleFor(a => a.CodigoAluno, f => f.Random.Int(1000, 9999))
                .RuleFor(a => a.CodigoTurma, f => f.Random.Int(1, 100));
        }

        [Fact]
        public void Construtor_QuandoHttpClientFactoryNulo_DeveLancarArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ObterAlunosTurmaPapQueryHandler(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("httpClientFactory");
        }

        [Fact]
        public async Task Handle_QuandoAnoLetivoInformado_DeveChamarUrlCorretaERetornarDados()
        {
            // Arrange
            const int anoLetivo = 2024;
            var query = new ObterAlunosTurmaPapQuery(anoLetivo);
            var alunosMock = _alunoFaker.Generate(3);
            var jsonResponse = JsonConvert.SerializeObject(alunosMock);
            var urlEsperada = string.Format(ServicosEolConstants.URL_ALUNOS_TURMAS_PAP_ANO_LETIVO, anoLetivo);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().Contains(urlEsperada)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
                });

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(alunosMock);
        }

        [Fact]
        public async Task Handle_QuandoAnoLetivoForAnoAtual_DeveChamarUrlDeAnoCorrenteERetornarDados()
        {
            // Arrange
            var query = new ObterAlunosTurmaPapQuery(DateTime.Now.Year);
            var alunosMock = _alunoFaker.Generate(2);
            var jsonResponse = JsonConvert.SerializeObject(alunosMock);
            var urlEsperada = ServicosEolConstants.URL_ALUNOS_TURMAS_PAP_ANO_CORRENTE;

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().Contains(urlEsperada)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
                });

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(alunosMock);
        }

        [Fact]
        public async Task Handle_QuandoApiRetornaErro_DeveLancarExcecao()
        {
            // Arrange
            var query = new ObterAlunosTurmaPapQuery(2023);

            _httpMessageHandlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.InternalServerError,
                   Content = new StringContent("Erro interno simulado")
               });

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("Não foi possível obter os alunos PAP. Erro: InternalServerError*");
        }

        [Fact]
        public async Task Handle_QuandoApiRetornaSucessoComConteudoVazio_DeveRetornarListaVazia()
        {
            // Arrange
            var query = new ObterAlunosTurmaPapQuery(2023);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                     "SendAsync",
                     ItExpr.IsAny<HttpRequestMessage>(),
                     ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }
    }
}
