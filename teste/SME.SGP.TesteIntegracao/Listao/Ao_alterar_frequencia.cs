using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_alterar_frequencia : ListaoTesteBase
    {
        protected Ao_alterar_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        [Fact(DisplayName = "Listão - Alteração de frequência pelo professor titular.")]
        public async Task Alteracao_de_frequencia_pelo_professor_titular()
        {
            
        } 
        
        [Fact(DisplayName = "Alteração de frequência pelo professor CJ")]
        public async Task Alteracao_de_frequencia_pelo_professor_cj()
        {
            
        }

        [Fact(DisplayName = "Alteração de frequência pelo CP")]
        public async Task Alteracao_de_frequencia_pelo_cp()
        {
            
        }
        
        [Fact(DisplayName = "Alteração de frequência pelo Diretor")]
        public async Task Alteracao_de_frequencia_pelo_diretor()
        {
            
        }
    }
}