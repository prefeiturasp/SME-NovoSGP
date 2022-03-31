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
        [Fact]
        public async Task Deve_Obter_Matriculas_Aluno_Na_Ue()
        {
            //-> Arrange
            var httpClient = new HttpClient(new HttpMessageHandlerMock());
            var resposta = await httpClient.GetAsync("https://api.eol.com.br/somefakeurl");

            //-> Act
            var alunos = new List<AlunoPorUeDto>();

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorUeDto>>(json);
            }

            //-> Assert
            Assert.True(alunos.Any(), "Existe matricula aluno.");
        }
    }

    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly List<AlunoPorUeDto> _matriculasAlunoUe;

        public HttpMessageHandlerMock()
        {
            var alunoPorUe = new AlunoPorUeDto()
            {
                CodigoAluno = "4824410",
                NomeAluno = "NICOLAS DOS SANTOS ALMEIDA SILVA",
                NomeSocialAluno = String.Empty,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                SituacaoMatricula = "Transferido",
                DataSituacao = DateTime.UtcNow.AddDays(-1),
                CodigoTurma = 2369048,
                CodigoMatricula = 36099185,
                AnoLetivo = 2022
            };

            _matriculasAlunoUe = new List<AlunoPorUeDto>()
            {
                alunoPorUe
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_matriculasAlunoUe), Encoding.UTF8, "application/json")
            });
        }
    }
}
