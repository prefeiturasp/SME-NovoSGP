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
            variaveis.Add("TokenAtual", GerarTokenValido("48e1e074-37d6-e911-abd6-f81654fe895d"));
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "1111111", Type = "rf" },
                new InternalClaim { Value = "48e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" },
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
    }
}
