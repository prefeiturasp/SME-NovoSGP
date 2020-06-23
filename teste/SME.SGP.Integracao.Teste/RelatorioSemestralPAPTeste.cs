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
    public class RelatorioSemestralPAPTeste
    {
        private readonly TestServerFixture _fixture;

        public RelatorioSemestralPAPTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact]
        //public void DeveSalvarRelatorioSemestralAluno()
        //{
        //    _fixture._clientApi.DefaultRequestHeaders.Clear();
        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { }));

        //    var dto = new RelatorioSemestralAlunoPersistenciaDto()
        //    {
        //        RelatorioSemestralAlunoId = 0,
        //        RelatorioSemestralId = 0,
        //        Secoes = new List<RelatorioSemestralAlunoSecaoDto>()
        //        {
        //            new RelatorioSemestralAlunoSecaoDto(1, "", "", true, "Teste 1"),
        //            new RelatorioSemestralAlunoSecaoDto(2, "", "", true, "Teste 2"),
        //            new RelatorioSemestralAlunoSecaoDto(3, "", "", true, "Teste 3"),
        //            new RelatorioSemestralAlunoSecaoDto(4, "", "", true, "Teste 4"),
        //            new RelatorioSemestralAlunoSecaoDto(5, "", "", false, "Teste 5"),
        //        }
        //    };

        //    var jsonParaPost = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        //    var postResult = _fixture._clientApi.PostAsync($"api/v1/relatorios/pap/semestral/turmas/321/semestres/1/alunos/123", jsonParaPost).Result;

        //    Assert.True(postResult.IsSuccessStatusCode);
        //}
    }
}
