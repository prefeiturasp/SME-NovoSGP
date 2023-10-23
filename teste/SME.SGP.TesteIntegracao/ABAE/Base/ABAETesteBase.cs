using System;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions.Brazil;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

        protected async Task CriarCadastroAcessoABAE(int quantidade = 1)
        {
            var cadastrosAcessosABAE = GerarCadastroAcessoABAE().Generate(quantidade);

            foreach (var cadastroAcessoAbae in cadastrosAcessosABAE)
                await InserirNaBase(cadastroAcessoAbae);
        }
        
        protected static Faker<CadastroAcessoABAE> GerarCadastroAcessoABAE()
        {
            var faker = new Faker<CadastroAcessoABAE>("pt_BR");
            faker.RuleFor(x => x.UeId, f => UE_ID_1);
            faker.RuleFor(x => x.Nome, f => f.Person.FullName);
            faker.RuleFor(x => x.Cpf, f => f.Person.Cpf());
            faker.RuleFor(x => x.Email, f => f.Person.Email);
            faker.RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber("(##) #####-####"));
            faker.RuleFor(x => x.Situacao, f => true);
            faker.RuleFor(x => x.Cep, f => f.Address.ZipCode());
            faker.RuleFor(x => x.Endereco, f => $"{f.Address.StreetSuffix()} {f.Address.StreetAddress()}");
            faker.RuleFor(x => x.Numero, f => int.Parse(f.Address.BuildingNumber()));
            faker.RuleFor(x => x.Complemento, f => f.Address.SecondaryAddress());
            faker.RuleFor(x => x.Bairro, f => f.Address.County());
            faker.RuleFor(x => x.Cidade, f => "São Paulo");
            faker.RuleFor(x => x.Estado, f => "SP");
            AuditoriaFaker(faker);
            return faker;
        }
        
        protected static Faker<CadastroAcessoABAEDto> GerarCadastroAcessoABAEDto()
        {
            var faker = new Faker<CadastroAcessoABAEDto>("pt_BR");
            faker.RuleFor(x => x.UeId, f => UE_ID_1);
            faker.RuleFor(x => x.Nome, f => f.Person.FullName);
            faker.RuleFor(x => x.Cpf, f => f.Person.Cpf());
            faker.RuleFor(x => x.Email, f => f.Person.Email);
            faker.RuleFor(x => x.Telefone, f => f.Phone.PhoneNumber("(##) #####-####"));
            faker.RuleFor(x => x.Situacao, f => true);
            faker.RuleFor(x => x.Cep, f => f.Address.ZipCode());
            faker.RuleFor(x => x.Endereco, f => $"{f.Address.StreetSuffix()} {f.Address.StreetAddress()}");
            faker.RuleFor(x => x.Numero, f => int.Parse(f.Address.BuildingNumber()));
            faker.RuleFor(x => x.Complemento, f => f.Address.SecondaryAddress());
            faker.RuleFor(x => x.Bairro, f => f.Address.County());
            faker.RuleFor(x => x.Cidade, f => "São Paulo");
            faker.RuleFor(x => x.Estado, f => "SP");
            return faker;
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
        
        protected IObterPaginadoCadastroAcessoABAEUseCase ObterServicoObterPaginadoCadastroAcessoABAEUseCase()
        {
            return ServiceProvider.GetService<IObterPaginadoCadastroAcessoABAEUseCase>();
        } 
    }
}