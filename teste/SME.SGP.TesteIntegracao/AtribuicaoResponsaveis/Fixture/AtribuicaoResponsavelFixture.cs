using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtribuicaoResponsaveis
{
    public class AtribuicaoResponsavelFixture : CollectionFixture
    {
        protected override void RegistrarMocks(IServiceCollection services)
        {
            base.RegistrarMocks(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSupervisorPorCodigoQuery, IEnumerable<SupervisoresRetornoDto>>), typeof(ObterSupervisorPorCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }
    }

    [CollectionDefinition("AtribuicaoResponsaveis")]
    public class AtribuicaoResponsavelCollection : ICollectionFixture<AtribuicaoResponsavelFixture>
    { }
}
