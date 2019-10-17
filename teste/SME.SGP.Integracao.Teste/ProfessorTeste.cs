using Newtonsoft.Json;
using SME.SGP.Dominio;
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

        [Fact, Order(5)]
        public async void DeveObterDisciplinasDoProfessorPorTurma()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { }));

            var getResult = await _fixture._clientApi.GetAsync("api/v1/professores/6082840/turmas/1982186/disciplinas/");

            Assert.True(getResult.IsSuccessStatusCode);
            var disciplinas = JsonConvert.DeserializeObject<IEnumerable<DisciplinaDto>>(await getResult.Content.ReadAsStringAsync());
            Assert.True(disciplinas != null);
        }

        [Theory, Order(6)]
        [InlineData("6082840", "095346", "2019")]
        [InlineData("5512557", "095346", "2019")]
        [InlineData("5773067", "095346", "2019")]
        [InlineData("7866089", "017272", "2019")]
        [InlineData("8029474", "095401", "2019")]
        public void DeveObterTurmasAtribuidasAoProfessorPorEscolaEAno(string rf, string escola, string ano)
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { }));

            var getResult = _fixture._clientApi.GetAsync($"api/v1/professores/{rf}/escolas/{escola}/turmas/anos-letivos/{ano}").Result;

            Assert.True(getResult.IsSuccessStatusCode);
            var turmas = JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.True(turmas != null);
        }

        [Theory, Order(7)]
        [InlineData("8029474", "095346", "2019")]
        public void NaoDeveObterTurmasAtribuidasAoProfessorEDeveRetornarSemErro(string rf, string escola, string ano)
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { }));

            var getResult = _fixture._clientApi.GetAsync($"api/v1/professores/{rf}/escolas/{escola}/turmas/anos-letivos/{ano}").Result;

            Assert.True(getResult.IsSuccessStatusCode);
            var turmas = JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.True(turmas == null);
        }
    }
}