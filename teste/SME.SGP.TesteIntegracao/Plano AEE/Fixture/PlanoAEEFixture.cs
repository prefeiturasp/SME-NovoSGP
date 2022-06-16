using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.TesteIntegracao.Plano_AEE
{
    public class PlanoAEEFixture : CollectionFixture
    {
        protected override void RegistrarMocks(IServiceCollection services)
        {
            base.RegistrarMocks(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorDreEolQueryHandlerFake), ServiceLifetime.Scoped));
        }
    }

    [CollectionDefinition("PlanoAEE")]
    public class PlanoAEECollection : ICollectionFixture<PlanoAEEFixture>
    {
    }
}
