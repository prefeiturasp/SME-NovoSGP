using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterMatriculasAlunoNaUEQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly ObterMatriculasAlunoNaUEQueryHandler _obterMatriculasAlunoNaUEQueryHandler;

        public ObterMatriculasAlunoNaUEQueryHandlerTeste()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _obterMatriculasAlunoNaUEQueryHandler = new ObterMatriculasAlunoNaUEQueryHandler(_httpClientFactory.Object);
        }

        [Fact]
        public async Task Deve_Obter_Mstriculas_Aluno_Na_Ue()
        {
            //-> Arrange
            var alunoPorUe = new AlunoPorUeDto()
            {
                CodigoAluno = "111",
                NomeAluno = "ALUNO TESTE",
                NomeSocialAluno = String.Empty,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                SituacaoMatricula = "Transferido",
                DataSituacao = DateTime.UtcNow.AddDays(-1),
                CodigoTurma = 1,
                CodigoMatricula = 1,
                AnoLetivo = 2022
            };

            var _matriculasAlunoUe = new List<AlunoPorUeDto>()
            {
                alunoPorUe
            };

            var json = new StringContent(JsonConvert.SerializeObject(_matriculasAlunoUe), Encoding.UTF8, "application/json");

            var httpClient = new HttpClient(new HttpMessageHandlerStub(async (request, cancellationToken) =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = json
                };

                return await Task.FromResult(response);
            }))
            {
                BaseAddress = new Uri("https://api.eol.com.br/somefakeurl")
            };

            _httpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            //-> Act
            var retorno = await _obterMatriculasAlunoNaUEQueryHandler.Handle(new ObterMatriculasAlunoNaUEQuery("1", "111"), new CancellationToken());

            //-> Assert
            Assert.True(retorno.Any(), "Falha ao consultar matriculas do aluno no EOL.");
        }
    }

    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsync;

        public HttpMessageHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
        {
            _sendAsync = sendAsync;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _sendAsync(request, cancellationToken);
        }
    }
}
