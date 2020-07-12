using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class ComponentesCurricularesTeste
    {
        private readonly TestServerFixture fixture;

        public ComponentesCurricularesTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Obter Componente Curriculare Por Anos E Modalidade")]
        [Trait("ComponentesCurriculares", "Obter Componente Curriculare Por Anos E Modalidade")]
        public async void ObterComponenteCurricularePorAnosEModalidade()
        {
            // Arrange            
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
            
            var result = await fixture._clientApi.GetAsync($"api/v1/componentes-curriculares/modalidades/5/anos/2020/anos-escolares?anosEscolares=1");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}
