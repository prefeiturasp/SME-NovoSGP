using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
﻿using Newtonsoft.Json
using System.Net.Http;
using System.Text;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class DiarioBordoTeste
    {
        private readonly TestServerFixture fixture;

        public DiarioBordoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }
      
        [Fact(DisplayName = "Obter Diário de Bordo")]
        [Trait("DiarioBordo", "Obter Diário de Bordo")]
        public async void Obter()
        {
            // Arrange
            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.DDB_C }));

            var id = "31886";
            var result = await fixture._clientApi.GetAsync($"api/v1/diario-bordo/{id}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));

        [Fact, Order(1)]
        public void Deve_Inserir_Diario_De_Bordo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.ACJ_A })); //TODO Colocar o permissionamento correto quando for criado

            var diarioBordoDto = new InserirDiarioBordoDto();

            var jsonParaPost = new StringContent(TransformarEmJson(diarioBordoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = fixture._clientApi.PostAsync("api/v1/diario-bordo/", jsonParaPost).Result;
            var result = postResult.Content.ReadAsStringAsync().Result;

            Assert.True(postResult.IsSuccessStatusCode, result);
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
