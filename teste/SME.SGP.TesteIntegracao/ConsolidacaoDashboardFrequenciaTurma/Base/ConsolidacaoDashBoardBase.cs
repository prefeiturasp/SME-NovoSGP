
using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConsolidacaoDashboardFrequenciaTurma.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.ConsolidacaoDashboardFrequenciaTurma
{
    public abstract class ConsolidacaoDashBoardBase : TesteBaseComuns
    {
        protected ConsolidacaoDashBoardBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosDentroPeriodoQueryHandlerFake), ServiceLifetime.Scoped));
        }        
    }
}