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
    public class DevolutivaTeste
    {
        private readonly TestServerFixture fixture;

        public DevolutivaTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }


        [Fact]
        public async void Deve_Obter_Sugestao_DataInicio()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_C }));

            string turmaCodigo = "1";
            long componenteCurricularId = 1105;
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/devolutivas/turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/sugestao");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Fact]
        public async void Deve_Obter_Devolutiva_Por_Id()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DE_C }));

            long devolutivaId = 1;
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/devolutivas/{devolutivaId}");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
