using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.RegistroColetivoNAAPA.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroColetivoNAAPA
{
    public class Ao_obter_tipo_reuniao_naapa : RegistroColetivoTesteBase
    {
        public Ao_obter_tipo_reuniao_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Registro Coletivo - Obter tipo de reunião")]
        public async Task Ao_obter_tipo_reuniao()
        {
            await CarregarTipoDeReuniao();

            var useCase = ServiceProvider.GetService<IObterTiposDeReuniaoUseCase>();
            var tiposReuniao = await useCase.Executar();

            tiposReuniao.ShouldNotBeNull();
            tiposReuniao.Count().ShouldBe(10);
        }
    }
}
