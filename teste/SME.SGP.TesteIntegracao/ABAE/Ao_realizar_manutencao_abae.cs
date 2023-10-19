using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ABAE.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Ocorrencia
{
    public class Ao_realizar_manutencao_abae : ABAETesteBase
    {
        public Ao_realizar_manutencao_abae(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "ABAE - Inserir")]
        public async Task Ao_inserir_cadastro_acesso_ABAE()
        {
            await CriarDadosBasicos();

            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();
            
            var dtoIncluir = new CadastroAcessoABAEDto
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
                Estado = "SP"
            };
            
            var retorno = await useCase.Executar(dtoIncluir);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Nome.ShouldBe(dtoIncluir.Nome);
            retorno.Cpf.ShouldBe(dtoIncluir.Cpf);
            retorno.Email.ShouldBe(dtoIncluir.Email);
            retorno.Telefone.ShouldBe(dtoIncluir.Telefone);
            retorno.Situacao.ShouldBe(dtoIncluir.Situacao);
            retorno.Cep.ShouldBe(dtoIncluir.Cep);
            retorno.Endereco.ShouldBe(dtoIncluir.Endereco);
            retorno.Numero.ShouldBe(dtoIncluir.Numero);
            retorno.Complemento.ShouldBe(dtoIncluir.Complemento);
            retorno.Cidade.ShouldBe(dtoIncluir.Cidade);
            retorno.Estado.ShouldBe(dtoIncluir.Estado);
        }
        
        [Fact(DisplayName = "ABAE - Não pode inserir com cpf duplicado")]
        public async Task Ao_inserir_com_cpf_duplicado_deve_gerar_excecao()
        {
            await CriarDadosBasicos();
            
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
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();
            
            var dtoIncluir = new CadastroAcessoABAEDto
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
                Estado = "SP"
            };
            
            await useCase.Executar(dtoIncluir).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "ABAE - Não pode alterar o cpf")]
        public async Task Ao_alterar_o_cpf_deve_gerar_excecao()
        {
            await CriarDadosBasicos();

            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();

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
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            var dtoAlterar = new CadastroAcessoABAEDto
            {
                Id = 1,
                UeId = 1,
                Nome = USUARIO_LOGADO_NOME,
                Cpf = "002.003.004-01",
                Email = "email1@email1.com",
                Telefone = "11 1999-9999",
                Situacao = true,
                Cep = "00000-001",
                Endereco = "Endereço DEF",
                Numero = 11,
                Complemento = "Com DEF",
                Cidade = "São Paulo ",
                Estado = "SP "
            };
            
            await useCase.Executar(dtoAlterar).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "ABAE - Não pode alterar a UE")]
        public async Task Ao_alterar_o_ue_deve_gerar_excecao()
        {
            await CriarDadosBasicos();

            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();

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
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            var dtoAlterar = new CadastroAcessoABAEDto
            {
                Id = 1,
                UeId = 2,
                Nome = USUARIO_LOGADO_NOME,
                Cpf = "001.002.003-01",
                Email = "email1@email1.com",
                Telefone = "11 1999-9999",
                Situacao = true,
                Cep = "00000-001",
                Endereco = "Endereço DEF",
                Numero = 11,
                Complemento = "Com DEF",
                Cidade = "São Paulo ",
                Estado = "SP "
            };
            
            await useCase.Executar(dtoAlterar).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "ABAE - Alterar")]
        public async Task Ao_alterar()
        {
            await CriarDadosBasicos();

            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();

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
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            var dtoAlterar = new CadastroAcessoABAEDto
            {
                Id = 1,
                UeId = 1,
                Nome = "Nome de usuário alterado",
                Cpf = "001.002.003-01",
                Email = "email1@email1.com",
                Telefone = "11 1999-9999",
                Situacao = true,
                Cep = "00000-001",
                Endereco = "Endereço DEF",
                Numero = 11,
                Complemento = "Com DEF",
                Cidade = "São Paulo ",
                Estado = "SP "
            };
            
            var retorno = await useCase.Executar(dtoAlterar);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(dtoAlterar.Id);
            retorno.Nome.ShouldBe(dtoAlterar.Nome);
            retorno.Cpf.ShouldBe(dtoAlterar.Cpf);
            retorno.Email.ShouldBe(dtoAlterar.Email);
            retorno.Telefone.ShouldBe(dtoAlterar.Telefone);
            retorno.Situacao.ShouldBe(dtoAlterar.Situacao);
            retorno.Cep.ShouldBe(dtoAlterar.Cep);
            retorno.Endereco.ShouldBe(dtoAlterar.Endereco);
            retorno.Numero.ShouldBe(dtoAlterar.Numero);
            retorno.Complemento.ShouldBe(dtoAlterar.Complemento);
            retorno.Cidade.ShouldBe(dtoAlterar.Cidade);
            retorno.Estado.ShouldBe(dtoAlterar.Estado);
        }
    }
}