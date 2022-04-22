using SME.SGP.Api;
using SME.SGP.TesteIntegracao.IntegracaoAPI.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.IntegracaoAPI._01___Testes
{
    [Collection(nameof(TesteIntegracaoApiFixtureCollection))]
    public class ConselhoClasseTeste
    {
        private readonly TesteIntegracaoFixture<Startup> _integrationTestFixture;
        const string TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiNzQ5NTA0OCIsImxvZ2luIjoiNzQ5NTA0OCIsIm5vbWUiOiJGQUJJQU5BIFJPQkVSVEEgR1VJTUFSQUVTIERPIFJFR08iLCJyZiI6Ijc0OTUwNDgiLCJwZXJmaWwiOiI0NGUxZTA3NC0zN2Q2LWU5MTEtYWJkNi1mODE2NTRmZTg5NWQiLCJyb2xlcyI6WyJTX0MiLCJTX0kiLCJTX0UiLCJTX0EiLCI1IiwiQl9DIiwiQ19DIiwiQUNKX0MiLCJOQ19DIiwiTkNfSSIsIk5DX0UiLCJOQ19BIiwiUEFfQyIsIlBBX0kiLCJQQV9FIiwiUEFfQSIsIlBEQ19DIiwiUERDX0kiLCJQRENfRSIsIlBEQ19BIiwiTl9DIiwiTl9JIiwiTl9FIiwiTl9BIiwiNDYiLCJBU19DIiwiTV9DIiwiTV9JIiwiTV9FIiwiTV9BIiwiUlBfQyIsIlJQX0kiLCJDUF9DIiwiQ1BfSSIsIkNQX0UiLCJDUF9BIiwiUEVfQyIsIlBGQV9DIiwiUEZSX0MiLCJFX0MiLCJBREFQX0MiLCJDQV9DIiwiQ0FfSSIsIkNBX0UiLCJDQV9BIiwiUkdQX0MiLCJSR1BfSSIsIlJHUF9FIiwiUkdQX0EiLCJSUEdfQyIsIkZCX0MiLCJGQl9JIiwiRkJfRSIsIkZCX0EiLCJQRl9DIiwiUEZfSSIsIlBGX0UiLCJQRl9BIiwiUFRfQyIsIlBUX0kiLCJQVF9FIiwiUFRfQSIsIkNDX0MiLCJDQ19JIiwiQ0NfRSIsIkNDX0EiLCJDT19DIiwiQ09fSSIsIkNPX0UiLCJDT19BIiwiUlBTX0MiLCJSUFNfSSIsIlJQU19FIiwiUlBTX0EiLCJQREFfQyIsIlBEQV9JIiwiUERBX0UiLCJQREFfQSIsIkFGUl9DIiwiSEVfQyIsIkZGX0MiLCJSUEZfQyIsIlJQQ19DIiwiRERCX0MiLCJEREJfSSIsIkREQl9FIiwiRERCX0EiLCJDSV9DIiwiQ0lfSSIsIkNJX0UiLCJDSV9BIiwiREVfSSIsIkRFX0UiLCJERV9BIiwiREVfQyIsIlJOQ0ZfQyIsIlJDQV9DIiwiUlBPQV9JIiwiUlBPQV9DIiwiUkNHX0MiLCJSRFVfQyIsIkRQVV9DIiwiRFBVX0kiLCJEUFVfRSIsIkRQVV9BIiwiUkROX0MiLCJSREVfQSIsIlJEQV9DIiwiUkFDSl9DIiwiUkRFX0MiLCJSTENfQyIsIlJDUF9DIiwiUkVJX0MiLCJPQ09fQyIsIk9DT19JIiwiT0NPX0UiLCJPQ09fQSIsIkFFRV9DIiwiQUVFX0kiLCJBRUVfRSIsIkFFRV9BIiwiQUZRX0MiLCJQQUVFX0MiLCJQQUVFX0kiLCJQQUVFX0UiLCJQQUVFX0EiLCJSQUFfQyIsIlJBQV9JIiwiUkFBX0UiLCJSQUFfQSIsIlJEX0MiLCJEQUVFX0MiLCJEUklfQyIsIkRGX0MiLCJESUVfQyIsIkFDRl9DIiwiRFJJTl9DIiwiREJfQyIsIkRBQV9DIiwiREZFX0MiLCJSQUNGX0MiLCJBQlJfQyIsIkREX0MiLCJSUlBfQyIsIkxfQyIsIkxfSSIsIkxfRSIsIkxfQSJdLCJuYmYiOjE2NTA2NDg5NzEsImV4cCI6MTY1MDY5MjE3MSwiaXNzIjoiTm92byBTR1AiLCJhdWQiOiJQcmVmZWl0dXJhIGRlIFNhbyBQYXVsbyJ9.s7gF2TVc_rMBkwveL2uZsWXWazg7GTpnYg6AXfC-i0o";
        public ConselhoClasseTeste(TesteIntegracaoFixture<Startup> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            _integrationTestFixture.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + TOKEN);
        }


        [Fact]
        public async Task Deve_Obter_Total_Aulas_Por_Aluno_Turma()
        {
            const string codigoAluno = "5854736";
            const string codigoTurma = "2123463";
            var requisicao = await _integrationTestFixture.Client.GetAsync($"conselhos-classe/ObterTotalAulas/aluno/{codigoAluno}/turma/{codigoTurma}");
            var resposta = await requisicao.Content.ReadAsStringAsync();

            Assert.True(requisicao.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Deve_Obter_Total_Aulas_Sem_Frequencia_Por_Turma()
        {
            const string codigoTurma = "2370993";

            var requisicao = await _integrationTestFixture.Client.GetAsync($"conselhos-classe/ObterTotalAulasSemFrequencia/turma/{codigoTurma}");
            var resposta = await requisicao.Content.ReadAsStringAsync();

            Assert.True(requisicao.IsSuccessStatusCode);
        }
    }
}
