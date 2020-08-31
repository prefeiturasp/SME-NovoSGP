using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class CartaIntencoesTeste
    {
        private readonly TestServerFixture fixture;

        public CartaIntencoesTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task Deve_Obter_Carta_Intencoes()
        {
            // Arrange 
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.CI_C }));

            var turmaCodigo = "123";
            var componenteCurricularId = 512;

            // Act
            var result = await fixture._clientApi.GetAsync($"api/v1/carta-intencoes/turmas/{turmaCodigo}/componente-curricular/{componenteCurricularId}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact]
        public async Task Deve_Obter_Carta_Intencoes_Observacao()
        {
            // Arrange 
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.CI_C }));

            var turmaId = 1;
            var componenteCurricularId = 512;

            // Act
            var result = await fixture._clientApi.GetAsync($"api/v1/carta-intencoes/turmas/{turmaId}/componente-curricular/{componenteCurricularId}/observacoes");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact]
        public void Deve_Inserir_Carta_Intencoes_Observacao()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_I }));

            var turmaId = 605357;
            var componenteCurricularId = 512;

            SalvarCartaIntencoesObservacaoDto dto = new SalvarCartaIntencoesObservacaoDto()
            {
                Observacao = "Teste de Inclusão de observação na carta de intenção..."
            };

            StringContent jsonParaPost = new StringContent(TransformarEmJson(dto), UnicodeEncoding.UTF8, "application/json");
            var postResult = fixture._clientApi.PostAsync($"api/v1/carta-intencoes/turmas/{turmaId}/componente-curricular/{componenteCurricularId}/observacoes", jsonParaPost).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        public void Deve_Alterar_Carta_Intencoes_Observacao()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_I }));

            var turmaId = 605357;
            var componenteCurricularId = 512;
            var observacaoId = 1;

            AlterarCartaIntencoesObservacaoDto dto = new AlterarCartaIntencoesObservacaoDto()
            {
                Observacao = "Teste de Alteração de observação na carta de intenção..."
            };

            StringContent jsonParaPost = new StringContent(TransformarEmJson(dto), UnicodeEncoding.UTF8, "application/json");
            var postResult = fixture._clientApi.PostAsync($"api/v1/carta-intencoes/turmas/{turmaId}/componente-curricular/{componenteCurricularId}/observacoes/{observacaoId}", jsonParaPost).Result;

            Assert.True(fixture.ValidarStatusCodeComSucesso(postResult));
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
