using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_pesquisar_responsavel_plano : PlanoAEETesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_pesquisar_responsavel_plano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact(DisplayName = "Plano AEE - Deve retornar o responsável pelo plano aee por ue")]
        public async Task Deve_retornar_responsavel_pelo_planoaee_por_ue()
        {
            await _builder.CriaItensComunsEja(); 

            var useCase = ServiceProvider.GetService<IPesquisaResponsavelPlanoPorDreUEUseCase>();
            var filtro = new FiltroPesquisaFuncionarioDto()
            {
                CodigoTurma = "1"
            };

            var pagina = await useCase.Executar(filtro);

            pagina.ShouldNotBeNull();
            pagina.Items.ShouldNotBeNull();
            pagina.Items.Count().ShouldBeGreaterThanOrEqualTo(1);
        }
    }
}
