using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class AnotacaoFrequenciaAlunoTeste
    {
        private readonly TestServerFixture fixture;

        public AnotacaoFrequenciaAlunoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async void Deve_Obter_Anotacao_Aluno()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { })); // TODO ajustar quando o permissionamento estiver pronto

            var codigoAluno = "123123";
            var aulaId = 100;
            var result = await fixture._clientApi.GetAsync($"api/v1/anotacoes/alunos/{codigoAluno}/aulas/{aulaId}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
