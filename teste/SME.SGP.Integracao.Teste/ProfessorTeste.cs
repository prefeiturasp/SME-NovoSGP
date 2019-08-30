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
    }
}