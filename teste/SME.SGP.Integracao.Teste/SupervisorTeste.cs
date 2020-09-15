using SME.SGP.Infra.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class SupervisorTeste
    {
        private readonly TestServerFixture _fixture;

        public SupervisorTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }


        //TODO: CHAVE INTEGRAÇÃO API EOL
        //[Theory, Order(1)]
        //[InlineData("108100", "an", true, true)]
        //[InlineData("108100", "ma", true, true)]
        //[InlineData("108100", "xy", false, true)]
        //[InlineData("108100", "", true, true)]
        //[InlineData("108100", "a", false, false)]
        //public void Deve_Consultar_Supervisores_Por_Nome_e_Dre(string dreId, string parteNome, bool temSupervisores, bool sucesso)
        //{
        //    _fixture._clientApi.DefaultRequestHeaders.Clear();

        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.ASP_I, Permissao.ASP_A, Permissao.ASP_E, Permissao.ASP_C }));

        //    var postResult = _fixture._clientApi.GetAsync($"api/v1/supervisores/dre/{dreId}?nome={parteNome}").Result;

        //    Assert.Equal(sucesso, postResult.IsSuccessStatusCode);

        //    if (postResult.IsSuccessStatusCode)
        //    {
        //        var supervisorEscolasDto = SgpJsonSerializer.Deserialize<List<SupervisorDto>>(postResult.Content.ReadAsStringAsync().Result);
        //        Assert.Equal(temSupervisores, supervisorEscolasDto.Count > 0);
        //    }
        //}

        //[Fact, Order(2)]
        //public void DeveAtribuirEscolaAoSupervisor()
        //{
        //    _fixture._clientApi.DefaultRequestHeaders.Clear();

        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.ASP_I, Permissao.ASP_A, Permissao.ASP_E, Permissao.ASP_C }));

        //    var post = SgpJsonSerializer.Serialize(new AtribuicaoSupervisorUEDto
        //    {
        //        DreId = "108100",
        //        UESIds = new List<string> { "095346" },
        //        SupervisorId = "7827067"
        //    });

        //    var jsonParaPost = new StringContent(post, UnicodeEncoding.UTF8, "application/json");

        //    var postResult = _fixture._clientApi.PostAsync("api/v1/supervisores/atribuir-ue", jsonParaPost).Result;

        //    Assert.True(postResult.IsSuccessStatusCode);

        //    var getResult = _fixture._clientApi.GetAsync("api/v1/supervisores/7827067/dre/108100").Result;
        //    var supervisorEscolasDto = SgpJsonSerializer.Deserialize<List<SupervisorEscolasDto>>(getResult.Content.ReadAsStringAsync().Result);
        //    Assert.Contains(supervisorEscolasDto, c => c.Escolas.Any(e => e.Codigo == "095346"));
        //}

        //TODO FAZER TESTE COM CONSULTA DO SUPERVISOR, POREM AINDA NÃO HÁ ENDPOINT DE CADASTRO.

        private string TransformarEmJson(object model)
        {
            return SgpJsonSerializer.Serialize(model);
        }
    }
}