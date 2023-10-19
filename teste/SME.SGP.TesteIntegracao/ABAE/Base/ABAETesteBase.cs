using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.ABAE.Base
{
    public  abstract class ABAETesteBase : TesteBaseComuns
    {
        protected ABAETesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected async Task CriarDadosBasicos()
        {
            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);
        }

        protected ISalvarCadastroAcessoABAEUseCase ObterServicoSalvarCadastroAcessoABAEUseCase()
        {
            return ServiceProvider.GetService<ISalvarCadastroAcessoABAEUseCase>();
        } 
    }
}