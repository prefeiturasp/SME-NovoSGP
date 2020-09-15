using SME.SGP.Infra.Json;
using SME.SGP.Api.Controllers;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class ProfessorTeste
    {
        private readonly TestServerFixture _fixture;

        public ProfessorTeste(TestServerFixture fixture)
        {
            this._fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        //[Theory, Order(5)]
        //[InlineData("7913583", "7913583", "3fe1e074-37d6-e911-abd6-f81654fe895d", "2001395")]
        //[InlineData("7913583", "7913583", "3fe1e074-37d6-e911-abd6-f81654fe895d", "2001401")]
        //public async Task DeveObterDisciplinasDoProfessorPorTurma(string login, string rf, string perfil, string codigoTurma)
        //{

        //    //chamar a controller

        //    _fixture._clientApi.DefaultRequestHeaders.Clear();

        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_A, Permissao.PA_C }, login, rf, perfil));

        //    var getResult = await _fixture._clientApi.GetAsync($"api/v1/professores/{login}/turmas/{codigoTurma}/disciplinas/");

        //    Assert.True(getResult.IsSuccessStatusCode);
        //    var disciplinas = SgpJsonSerializer.Deserialize<IEnumerable<DisciplinaDto>>(await getResult.Content.ReadAsStringAsync());
        //    Assert.True(disciplinas != null);
        //}

            //TODO: CHAVE INTEGRAÇÃO API EOL
        //[Theory, Order(6)]
        //[InlineData("6082840", "095346", "2019")]
        //[InlineData("5512557", "095346", "2019")]
        //public void DeveObterTurmasAtribuidasAoProfessorPorEscolaEAno(string rf, string escola, string ano)
        //{

        // _fixture._clientApi.DefaultRequestHeaders.Clear();

        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_A, Permissao.PA_C }));

        //    var getResult = _fixture._clientApi.GetAsync($"api/v1/professores/{rf}/escolas/{escola}/turmas/anos-letivos/{ano}").Result;

        //    Assert.True(getResult.IsSuccessStatusCode);
        //    var turmas = SgpJsonSerializer.Deserialize<IEnumerable<TurmaDto>>(getResult.Content.ReadAsStringAsync().Result);
        //    Assert.True(turmas != null);
        //}

        [Theory, Order(7)]
        [InlineData("8029474", "095346", "2019")]
        public void NaoDeveObterTurmasAtribuidasAoProfessorEDeveRetornarSemErro(string rf, string escola, string ano)
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_A, Permissao.PA_C }));

            var getResult = _fixture._clientApi.GetAsync($"api/v1/professores/{rf}/escolas/{escola}/turmas/anos-letivos/{ano}").Result;

            Assert.True(getResult.IsSuccessStatusCode);
            var turmas = SgpJsonSerializer.Deserialize<IEnumerable<TurmaDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.True(turmas == null);
        }
    }
}