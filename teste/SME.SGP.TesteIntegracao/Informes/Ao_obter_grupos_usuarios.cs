using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Informes.Base;
using SME.SGP.TesteIntegracao.Informes.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Informes
{
    public class Ao_obter_grupos_usuarios : InformesBase
    {
        public Ao_obter_grupos_usuarios(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterGruposDeUsuariosQuery, IEnumerable<GruposDeUsuariosDto>>), typeof(ObterGruposDeUsuariosQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Informes - Obter grupos de usuários")]
        public async Task Obter_grupos_usuarios()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IObterGruposDeUsuariosUseCase>();

            var resultado = await useCase.Executar();
            resultado.ShouldNotBeNull();
            resultado.Count().ShouldBeGreaterThan(1);
        }
    }
}
