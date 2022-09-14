using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_pesquisar_aluno_plano_aee : PlanoAEETesteBase
    {
        public Ao_pesquisar_aluno_plano_aee(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            
        }
        
        [Fact]
        public async Task Selecionar_aluno_com_plano_validado()
        {
            //var useCase = ObterAlunosPorCodigoEolNomeUseCase();
            var useCase = ObterServicoSalvarPlanoAEEUseCase();
        }
        
    }
}