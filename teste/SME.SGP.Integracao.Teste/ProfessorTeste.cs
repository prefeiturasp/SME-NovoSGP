using Newtonsoft.Json;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    public class ProfessorTeste : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public ProfessorTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void DeveObterDisciplinasDoProfessorPorTurma()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            var getResult = fixture._clientApi.GetAsync("api/v1/professores/6082840/turmas/1982186/disciplinas/").Result;

            Assert.True(getResult.IsSuccessStatusCode);
            var disciplinas = JsonConvert.DeserializeObject<IEnumerable<DisciplinaDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.True(disciplinas != null);
        }

        [Theory]
        [InlineData("6082840", "095346", "2019")]
        [InlineData("5512557", "095346", "2019")]
        [InlineData("5773067", "095346", "2019")]
        [InlineData("7866089", "017272", "2019")]
        [InlineData("8029474", "095401", "2019")]
        public void DeveObterTurmasAtribuidasAoProfessorPorEscolaEAno(string rf, string escola, string ano)
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            var getResult = fixture._clientApi.GetAsync($"api/v1/professores/{rf}/escolas/{escola}/turmas/anos-letivos/{ano}").Result;

            Assert.True(getResult.IsSuccessStatusCode);
            var turmas = JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.True(turmas != null);
        }

        [Theory]
        [InlineData("8029474", "095346", "2019")]
        public void NaoDeveObterTurmasAtribuidasAoProfessorEDeveRetornarSemErro(string rf, string escola, string ano)
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            var getResult = fixture._clientApi.GetAsync($"api/v1/professores/{rf}/escolas/{escola}/turmas/anos-letivos/{ano}").Result;

            Assert.True(getResult.IsSuccessStatusCode);
            var turmas = JsonConvert.DeserializeObject<IEnumerable<TurmaDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.True(turmas == null);
        }
    }
}