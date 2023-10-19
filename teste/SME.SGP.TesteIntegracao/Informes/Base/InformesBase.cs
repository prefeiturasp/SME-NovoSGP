using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Informes.Base
{
    public class InformesBase : TesteBaseComuns
    {
        public InformesBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase()
        {
            await CriarDreUePerfilComponenteCurricular();
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
        }
    }
}
