using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Informe.Base
{
    public abstract class InformesBase : TesteBaseComuns
    {
        protected const long INFORME_ID_1 = 1;
        protected const long INFORME_ID_2 = 2;
        protected const long PERFIL_AD = 11;
        protected const long PERFIL_ADM_COTIC = 33;
        protected const long PERFIL_ADM_DRE = 14;
        protected const long PERFIL_ADM_SME = 32;
        protected const long PERFIL_ADM_UE = 8;

        public InformesBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase()
        {
            await CriarDreUePerfil();
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
        }
    }
}
