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
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { })); // TODO Ajustar quando permissionamento estiver ok

            string turmaCodigo = "1";
            long componenteCurricularId = 1105;
            HttpResponseMessage result = await fixture._clientApi.GetAsync($"api/v1/devolutivas/turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/sugestao");

            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
