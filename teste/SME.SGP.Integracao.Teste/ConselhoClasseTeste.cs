using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class ConselhoClasseTeste
    {
        private readonly TestServerFixture fixture;
        private readonly long _id = 1;



        public ConselhoClasseTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar detalhamento de nota")]
        [Trait("Conselho de Classe", "Detalhamento de Nota")]
        public async void Retornar_Detalhamento_De_Nota()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            var result = await fixture._clientApi.GetAsync($"api/v1/conselhos-classe/detalhamento/{_id}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }


        [Fact(DisplayName = "Obter Relatório Conselho Classe Aluno")]
        [Trait("Conselho de Classe", "Relatório Conselho Classe Aluno")]
        public async void Retornar_Relatorio_Conselho_Classe_Aluno()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.CC_C }));
            var result = await fixture._clientApi.GetAsync("api/v1/conselhos-classe/1/fechamentos/1/alunos/123/imprimir");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact(DisplayName = "Obter Relatório Conselho Classe Turma")]
        [Trait("Conselho de Classe", "Relatório Conselho Classe Turma")]
        public async void Retornar_Relatorio_Conselho_Classe_Turma()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.CC_C }));
            var result = await fixture._clientApi.GetAsync("api/v1/conselhos-classe/1/fechamentos/1/imprimir");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact(DisplayName = "Listar Pareceres Conclusivos")]
        [Trait("Conselho de Classe", "Pareceres Conclusivos")]
        public async void Lista_Pareceres_Conclusivos()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.RPC_C }));
            var result = await fixture._clientApi.GetAsync("api/v1/conselhos-classe/pareceres-conclusivos");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}