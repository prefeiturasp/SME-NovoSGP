using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_obter_lista_tipo_responsavel : TesteBase
    {
        public Ao_obter_lista_tipo_responsavel(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_obter_lista_de_responsaveis()
        {
            var useCase = ServiceProvider.GetService<IObterListaTipoReponsavelUseCase>();

            var resultados = await useCase.Executar(true);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(5);
        }
    }
}
