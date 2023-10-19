using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.ABAE.Base
{
    public  abstract class ABAETesteBase : TesteBaseComuns
    {
        protected ABAETesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected async Task CriarDadosBasicos(bool criarCadastroAcessoABAE = false)
        {
            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);

            if (criarCadastroAcessoABAE)
                await CriarCadastroAcessoABAE();
        }

        private async Task CriarCadastroAcessoABAE()
        {
            await InserirNaBase(new CadastroAcessoABAE()
            {
                UeId = 1,
                Nome = USUARIO_LOGADO_NOME,
                Cpf = "001.002.003-01",
                Email = "email@email.com",
                Telefone = "11 9999-9999",
                Situacao = true,
                Cep = "01000-001",
                Endereco = "Endereço ABC",
                Numero = 99,
                Complemento = "Complemento",
                Cidade = "São Paulo",
                Estado = "SP",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected ISalvarCadastroAcessoABAEUseCase ObterServicoSalvarCadastroAcessoABAEUseCase()
        {
            return ServiceProvider.GetService<ISalvarCadastroAcessoABAEUseCase>();
        }
        
        protected IExcluirCadastroAcessoABAEUseCase ObterServicoExcluirCadastroAcessoABAEUseCase()
        {
            return ServiceProvider.GetService<IExcluirCadastroAcessoABAEUseCase>();
        }
        
        protected IObterCadastroAcessoABAEUseCase ObterServicoObterCadastroAcessoABAEUseCase()
        {
            return ServiceProvider.GetService<IObterCadastroAcessoABAEUseCase>();
        } 
    }
}