using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TipoResponsavel
{
    public class Ao_obter_lista_tipo_responsavel : TesteBase
    {
        public Ao_obter_lista_tipo_responsavel(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_obter_lista_de_responsaveis()
        {
            await CriarUsuarioLogado();
            CriarClaimFundamental();

            var useCase = ServiceProvider.GetService<IObterListaTipoReponsavelUseCase>();

            var resultados = await useCase.Executar(true);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(5);
        }

        private async Task CriarUsuarioLogado()
        {
            await InserirNaBase(new Usuario
            {
                Login = "1111111",
                CodigoRf = "1111111",
                Nome = "NOME DO USUARIO 1",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private void CriarClaimFundamental()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "NOME DO USUARIO 1");
            variaveis.Add("UsuarioLogado", "1111111");
            variaveis.Add("RF", "1111111");
            variaveis.Add("login", "1111111");
            variaveis.Add("TokenAtual", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiODg1MDk3NiIsImxvZ2luIjoiODg1MDk3NiIsIm5vbWUiOiJFTElTQSBDUklTVElOQSBERSBPTElWRUlSQSBTQUJJTk8iLCJyZiI6Ijg4NTA5NzYiLCJwZXJmaWwiOiI2NWUxZTA3NC0zN2Q2LWU5MTEtYWJkNi1mODE2NTRmZTg5NWQiLCJyb2xlcyI6WyJCX0MiLCJDX0MiLCJOQ19DIiwiUEFfQyIsIlBEQ19DIiwiTl9DIiwiTl9JIiwiTl9FIiwiTl9BIiwiNDYiLCJNX0MiLCJNX0kiLCJNX0UiLCJNX0EiLCJSUF9DIiwiQ1BfQyIsIlBFX0MiLCJQRkFfQyIsIlBGUl9DIiwiRV9DIiwiQVJQX0MiLCJBUlBfSSIsIkFSUF9FIiwiQVJQX0EiLCJBREFQX0MiLCJDQV9DIiwiUkdQX0MiLCJSUEdfQyIsIkZCX0MiLCJQVF9DIiwiQ0NfQyIsIlJQU19DIiwiUERBX0MiLCJBRlJfQyIsIkhFX0MiLCJGRl9DIiwiUlBGX0MiLCJSUENfQyIsIkREQl9DIiwiQ0lfQyIsIk9DT19DIiwiUlBPQV9DIiwiREVfQyIsIlJOQ0ZfQyIsIlJDQV9DIiwiRFBVX0MiLCJSQUNKX0MiLCJSRUlfQyIsIkFFRV9DIiwiUEFFRV9DIiwiUklfQyIsIlJBQV9DIiwiUkRfQyIsIkFGUV9DIiwiREFFRV9DIiwiRFJJX0MiLCJERl9DIiwiRElFX0MiLCJERF9DIiwiRFJJTl9DIiwiREJfQyIsIkRBQV9DIiwiUlJQX0MiLCJMX0MiLCJSRk1fQyIsIkROQV9DIl0sIm5iZiI6MTY1MzQ4NzI2MCwiZXhwIjoxNjUzNTMwNDYwLCJpc3MiOiJOb3ZvIFNHUCIsImF1ZCI6IlByZWZlaXR1cmEgZGUgU2FvIFBhdWxvIn0.es--3yRVBfuFIAf0N0U01T_TAXtjG8NqXuMNgpP9oCU");
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "1111111", Type = "rf" },
                new InternalClaim { Value = "48e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" },                
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
    }
}
