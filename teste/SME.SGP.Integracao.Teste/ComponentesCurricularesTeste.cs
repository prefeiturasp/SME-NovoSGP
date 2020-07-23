using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

            var codigoUe = "094765";
            var result = await fixture._clientApi.GetAsync($"api/v1/componentes-curriculares/ues/{codigoUe}/modalidades/5/anos/2020/anos-escolares?anosEscolares=1");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
        //[Fact(DisplayName = "Retornar componentes curriculares")]
        //[Trait("Componentes Curriculares", "Obter Componentes Curriculares Por Turma E CodigoUe")]
        //public async void Obter_Componentes_Curriculares_PorTurma_E_CodigoUe()
        //{
        //    // Arrange
        //    FiltroComponentesCurricularesPorTurmaECodigoUeDto filtro = new FiltroComponentesCurricularesPorTurmaECodigoUeDto();
        //    var jsonParaPost = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
        //    // Act
        //    fixture._clientApi.DefaultRequestHeaders.Clear();
        //    fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));
        //    var result = await fixture._clientApi.PostAsync($"api/v1/componentes-curriculares/obter", jsonParaPost);

        //    // Assert
        //    Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        //}
    }
}
