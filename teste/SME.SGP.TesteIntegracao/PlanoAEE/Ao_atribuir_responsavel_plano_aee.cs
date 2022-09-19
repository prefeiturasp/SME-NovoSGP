using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_atribuir_responsavel_plano_aee : PlanoAEETesteBase
    {
        public Ao_atribuir_responsavel_plano_aee(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Com o Coordenador do CEFAI realizar atribuição do PAAI.")]
        public async Task Realizar_atribuicao_paai_com_usuario_cefai()
        {
            
        }
        
        [Fact(DisplayName = "Alterar o PAAI atribuído - A pendência deverá ser transferida para o novo PAAI")]
        public async Task Alterar_o_paai_atribuído()
        {
            
        }
    }
}