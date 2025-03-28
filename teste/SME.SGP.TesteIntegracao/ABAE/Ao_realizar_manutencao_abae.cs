using System.Collections.Generic;
using System.Linq;
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
            
            var dtoIncluir = GerarCadastroAcessoABAEDto().Generate();
            
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
            retorno.Bairro.ShouldBe(dtoIncluir.Bairro);
            retorno.Cidade.ShouldBe(dtoIncluir.Cidade);
            retorno.Estado.ShouldBe(dtoIncluir.Estado);
        }
        
        [Fact(DisplayName = "ABAE - Pode inserir com cpf duplicado em Ues diferentes")]
        public async Task Ao_inserir_com_cpf_duplicado_deve_gerar_excecao()
        {
            await CriarDadosBasicos(true);
            
            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();

            var cadastros = ObterTodos<CadastroAcessoABAE>();
            
            var dtoIncluir = GerarCadastroAcessoABAEDto().Generate();
            dtoIncluir.Cpf = cadastros.FirstOrDefault().Cpf;
            dtoIncluir.UeId = UE_ID_2;
            
            var retorno = await useCase.Executar(dtoIncluir);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(UE_ID_2);
            retorno.Nome.ShouldBe(dtoIncluir.Nome);
            retorno.Cpf.ShouldBe(dtoIncluir.Cpf);
            retorno.Email.ShouldBe(dtoIncluir.Email);
            retorno.Telefone.ShouldBe(dtoIncluir.Telefone);
            retorno.Situacao.ShouldBe(dtoIncluir.Situacao);
            retorno.Cep.ShouldBe(dtoIncluir.Cep);
            retorno.Endereco.ShouldBe(dtoIncluir.Endereco);
            retorno.Numero.ShouldBe(dtoIncluir.Numero);
            retorno.Complemento.ShouldBe(dtoIncluir.Complemento);
            retorno.Bairro.ShouldBe(dtoIncluir.Bairro);
            retorno.Cidade.ShouldBe(dtoIncluir.Cidade);
            retorno.Estado.ShouldBe(dtoIncluir.Estado);
        }
        
        [Fact(DisplayName = "ABAE - Não pode inserir com cpf duplicado em mesmas UEs")]
        public async Task Ao_inserir_com_cpf_duplicado_em_mesma_ues_deve_gerar_excecao()
        {
            await CriarDadosBasicos(true);
            
            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();

            var cadastros = ObterTodos<CadastroAcessoABAE>();
            
            var dtoIncluir = GerarCadastroAcessoABAEDto().Generate();
            dtoIncluir.Cpf = cadastros.FirstOrDefault().Cpf;
            
            await useCase.Executar(dtoIncluir).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "ABAE - Não pode alterar o cpf")]
        public async Task Ao_alterar_o_cpf_deve_gerar_excecao()
        {
            await CriarDadosBasicos(true);
            
            var dtoAlterar = GerarCadastroAcessoABAEDto().Generate();
            dtoAlterar.Id = 1;

            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();
            
            await useCase.Executar(dtoAlterar).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "ABAE - Não pode alterar a UE")]
        public async Task Ao_alterar_o_ue_deve_gerar_excecao()
        {
            await CriarDadosBasicos(true);
            
            var dtoAlterar = GerarCadastroAcessoABAEDto().Generate();
            dtoAlterar.Id = 1;
            dtoAlterar.UeId = 2;
            
            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();
            
            await useCase.Executar(dtoAlterar).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "ABAE - Alterar")]
        public async Task Ao_alterar()
        {
            await CriarDadosBasicos(true);

            var useCase = ObterServicoSalvarCadastroAcessoABAEUseCase();
            
            var dtoAlterar = GerarCadastroAcessoABAEDto().Generate();
            
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
            retorno.Bairro.ShouldBe(dtoAlterar.Bairro);
            retorno.Cidade.ShouldBe(dtoAlterar.Cidade);
            retorno.Estado.ShouldBe(dtoAlterar.Estado);
        }
        
        [Fact(DisplayName = "ABAE - Excluir logicamente")]
        public async Task Ao_excluir_logicamente()
        {
            await CriarDadosBasicos(true);

            var useCase = ObterServicoExcluirCadastroAcessoABAEUseCase();

            var retorno = await useCase.Executar(1);
            retorno.ShouldBeTrue();
            
            var cadastros = ObterTodos<CadastroAcessoABAE>();
            cadastros.Count.ShouldBe(1);
            cadastros.Any(a=> a.Excluido).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "ABAE - Não deve excluir cadastros inexistentes")]
        public async Task Ao_excluir_cadastro_inexistentes()
        {
            await CriarDadosBasicos(true);

            var useCase = ObterServicoExcluirCadastroAcessoABAEUseCase();
            
            await useCase.Executar(100).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "ABAE - Obter por Id")]
        public async Task Ao_obter_por_id()
        {
            await CriarDadosBasicos();
            
            var dtoInserir = GerarCadastroAcessoABAE().Generate();
            await InserirNaBase(dtoInserir);

            var useCase = ObterServicoObterCadastroAcessoABAEUseCase();
            
            var retorno = await useCase.Executar(1);
            retorno.ShouldNotBeNull();
            retorno.UeId.ShouldBe(dtoInserir.UeId);
            retorno.Nome.ShouldBe(dtoInserir.Nome);
            retorno.Cpf.ShouldBe(dtoInserir.Cpf);
            retorno.Email.ShouldBe(dtoInserir.Email);
            retorno.Telefone.ShouldBe(dtoInserir.Telefone);
            retorno.Situacao.ShouldBeTrue();
            retorno.Cep.ShouldBe(dtoInserir.Cep);
            retorno.Endereco.ShouldBe(dtoInserir.Endereco);
            retorno.Numero.ShouldBe(dtoInserir.Numero);
            retorno.Complemento.ShouldBe(dtoInserir.Complemento);
            retorno.Bairro.ShouldBe(dtoInserir.Bairro);
            retorno.Cidade.ShouldBe(dtoInserir.Cidade);
            retorno.Estado.ShouldBe(dtoInserir.Estado);
            retorno.CriadoEm.Date.ShouldBe(dtoInserir.CriadoEm.Date);
            retorno.CriadoPor.ShouldBe(dtoInserir.CriadoPor);
            retorno.CriadoRF.ShouldBe(dtoInserir.CriadoRF);
            retorno.AlteradoEm.Value.Date.ShouldBe(dtoInserir.AlteradoEm.Value.Date);
            retorno.AlteradoPor.ShouldBe(dtoInserir.AlteradoPor);
            retorno.AlteradoRF.ShouldBe(dtoInserir.AlteradoRF);
        }

        [Fact(DisplayName = "ABAE - Validar telefone")]
        public async Task Ao_validar_telefone()
        {
            "11 1212 1212".EhTelefoneValido().ShouldBeFalse();
            "(11) 41515-1212".EhTelefoneValido().ShouldBeTrue();
            "abab.com".EhTelefoneValido().ShouldBeFalse();
        }
        
        [Fact(DisplayName = "ABAE - Validar e-mail")]
        public async Task Ao_validar_email()
        {
            "ab@ab.com".EhEmailValido().ShouldBeTrue();
            "ab@ab.net".EhEmailValido().ShouldBeTrue();
            "ab@ab.com.br".EhEmailValido().ShouldBeTrue();
            "abab.com".EhEmailValido().ShouldBeFalse();
            "@abab.com".EhEmailValido().ShouldBeFalse();
            "@abab.net".EhEmailValido().ShouldBeFalse();
        }
        
        [Fact(DisplayName = "ABAE - Obter paginado")]
        public async Task Ao_obter_paginado()
        {
            await CriarDadosBasicos();

            await CriarCadastroAcessoABAE(22);

            var useCase = ObterServicoObterPaginadoCadastroAcessoABAEUseCase();

            var filtro = new FiltroDreIdUeIdNomeSituacaoABAEDto()
            {
                UeId = 1,
                Situacao = true,
            };
            
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(22);
        }

        [Fact(DisplayName = "ABAE - Obter paginado com filtro Dre/Ue")]
        public async Task Ao_obter_paginado_dre_ue()
        {
            var dresUes = new List<(string CodigoDre, string CodigoUe)> { (DRE_CODIGO_1, UE_CODIGO_1), (DRE_CODIGO_2, UE_CODIGO_2), (DRE_CODIGO_2, UE_CODIGO_3) };
            await CriarDadosBasicos(dresUes: dresUes);

            await CriarCadastroAcessoABAE(5, UE_ID_1);
            await CriarCadastroAcessoABAE(5, UE_ID_2);
            await CriarCadastroAcessoABAE(5, UE_ID_3);
            var useCase = ObterServicoObterPaginadoCadastroAcessoABAEUseCase();

            var filtro = new FiltroDreIdUeIdNomeSituacaoABAEDto()
            { Situacao = true };
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(15);
            retorno.Items.FirstOrDefault().Dre.ShouldBe("Nome Dre 1");
            retorno.Items.LastOrDefault().Dre.ShouldBe("Nome Dre 2");

            filtro = new FiltroDreIdUeIdNomeSituacaoABAEDto()
            { DreId = DRE_ID_1, Situacao = true };
            retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(5);

            filtro = new FiltroDreIdUeIdNomeSituacaoABAEDto()
            { UeId = UE_ID_1, Situacao = true };
            retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(5);

            filtro = new FiltroDreIdUeIdNomeSituacaoABAEDto()
            { DreId = DRE_ID_2, Situacao = true };
            retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(10);
            retorno.Items.FirstOrDefault().Ue.ShouldBe("NA Nome Ue 2");
            retorno.Items.LastOrDefault().Ue.ShouldBe("NA Nome Ue 3");
        }
    }
}