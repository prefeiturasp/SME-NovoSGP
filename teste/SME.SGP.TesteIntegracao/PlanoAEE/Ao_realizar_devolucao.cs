using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_realizar_devolucao : PlanoAEETesteBase
    {
        public Ao_realizar_devolucao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        }

        [Fact(DisplayName = "Com o CP editar um plano que está na situação Aguardando parecer da Coordenação e clicar em Devolver, preenchendo o campo de motivo")]
        public async Task Editar_plano_que_esta_na_situacao_aguardando_parecer_da_coordenacao()
        {
            //Arrange
            
            //Act
            
            //Assert
        }

        [Fact(DisplayName = "Com o Coordenador do CEFAI editar um plano que está na situação Aguardando atribuição de PAAI e clicar em Devolver, preenchendo o campo de motivo.")]
        public async Task Editar_um_plano_que_esta_na_situacao_aguardando_atribuicao_paai()
        {
            //Arrange
            
            //Act
            
            //Assert
        }
        [Fact(DisplayName = "Com o PAAI editar um plano que está na situação Aguardando parecer do CEFAI e clicar em Devolver, preenchendo o campo de motivo")]
        public async Task Editar_um_plano_que_esta_na_situacao_aguardando_parecer_do_cefai()
        {
            //Arrange
            
            //Act
            
            //Assert
        }
    }
}