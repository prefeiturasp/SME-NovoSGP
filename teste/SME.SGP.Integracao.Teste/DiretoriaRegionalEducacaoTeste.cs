using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class DiretoriaRegionalEducacaoTeste
    {
        private readonly TestServerFixture _fixture;

        public DiretoriaRegionalEducacaoTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(10)]
        public void Deve_Consultar_Dres()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { }));

            var postResult = _fixture._clientApi.GetAsync("api/v1/dres").Result;

            Assert.True(_fixture.ValidarStatusCodeComSucesso(postResult));
        }

        [Fact, Order(11)]
        public void Deve_Consultar_Escolas_Sem_Atribuicao()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { }));

            var postResult = _fixture._clientApi.GetAsync("api/v1/dres/18100/escolas/sem-atribuicao").Result;

            if (postResult.IsSuccessStatusCode)
            {
                var supervisorEscolasDto = JsonConvert.DeserializeObject<List<UnidadeEscolarDto>>(postResult.Content.ReadAsStringAsync().Result);
                Assert.True(supervisorEscolasDto.Count > 0);
            }
        }

        //TODO CRIAR TESTE INCLUINDO UM SUPERVISOR - ESCOLA - DRE e O MESMO NÃO DEVE VIR NA CONSULTA;
    }
}