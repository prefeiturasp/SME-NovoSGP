using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] {
            Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E
            })); // TODO ajustar quando o permissionamento estiver pronto

            var codigoAluno = "123123";
            var aulaId = 100;
            var result = await fixture._clientApi.GetAsync($"api/v1/anotacoes/alunos/{codigoAluno}/aulas/{aulaId}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }


        [Fact]
        public async void Deve_Salvar_Anotacao_Aluno()
        {
            // Arrange & Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] {
            Permissao.PDA_I, Permissao.PDA_A, Permissao.PDA_E
            })); // TODO ajustar quando o permissionamento estiver pronto

            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1771346,
                CodigoAluno = "6502235",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var result = await fixture._clientApi.PostAsync($"api/v1/anotacoes/alunos/", jsonParaPost);

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
