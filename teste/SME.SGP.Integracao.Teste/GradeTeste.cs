using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class GradeTeste
    {
        private readonly TestServerFixture _fixture;

        public GradeTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact]
        //public void DeveRetornarHorasGradeEHorasRestantes()
        //{
        //    _fixture._clientApi.DefaultRequestHeaders.Clear();
        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { }));

        //    var turma = 123;
        //    var disciplina = 7;

        //    var getResult = _fixture._clientApi.GetAsync($"api/v1/grade/aulas/{turma}/{disciplina}").Result;

        //    Assert.True(getResult.IsSuccessStatusCode);
        //}

    }
}
