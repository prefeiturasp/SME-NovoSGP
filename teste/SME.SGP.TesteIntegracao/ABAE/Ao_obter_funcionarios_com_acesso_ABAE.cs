using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ABAE.Base;
using SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ABAE
{
    public class Ao_obter_funcionarios_com_acesso_ABAE : ABAETesteBase
    {
        public Ao_obter_funcionarios_com_acesso_ABAE(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
                typeof(ObterFuncionariosPorDreEolQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "ABAE - Obter funcionários com acessos ABAE por dre")]
        public async Task Ao_obter_funcionarios_com_acesso_ABAE_por_dre()
        {
            await CriarDadosBasicos(true);
          
            var useCase = ServiceProvider.GetService<IObterFuncionariosUseCase>();

            var funcionarios = await useCase.Executar(new FiltroFuncionarioDto() { CodigoDRE = DRE_CODIGO_1 });

            funcionarios.ShouldNotBeNull();
            funcionarios.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "ABAE - Obter funcionários com acessos ABAE por ue")]
        public async Task Ao_obter_funcionarios_com_acesso_ABAE_por_ue()
        {
            await CriarDadosBasicos(true);

            var useCase = ServiceProvider.GetService<IObterFuncionariosUseCase>();

            var funcionarios = await useCase.Executar(new FiltroFuncionarioDto() { CodigoDRE = DRE_CODIGO_1, CodigoUE = UE_CODIGO_1 });

            funcionarios.ShouldNotBeNull();
            funcionarios.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "ABAE - Obter funcionários com acessos ABAE por cpf")]
        public async Task Ao_obter_funcionarios_com_acesso_ABAE_por_cpf()
        {
            await CriarDadosBasicos(true);
            await CriarCadastroAcessoABAE();

            var acessoAbae = ObterTodos<CadastroAcessoABAE>().FirstOrDefault();

            var useCase = ServiceProvider.GetService<IObterFuncionariosUseCase>();
            var funcionarios = await useCase.Executar(new FiltroFuncionarioDto() { CodigoDRE = DRE_CODIGO_1, CodigoRF = acessoAbae.Cpf });

            funcionarios.ShouldNotBeNull();
            funcionarios.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "ABAE - Obter funcionários com acessos ABAE por nome")]
        public async Task Ao_obter_funcionarios_com_acesso_ABAE_por_nome()
        {
            await CriarDadosBasicos(true);
            await CriarCadastroAcessoABAE();

            var acessoAbae = ObterTodos<CadastroAcessoABAE>().FirstOrDefault();

            var useCase = ServiceProvider.GetService<IObterFuncionariosUseCase>();
            var funcionarios = await useCase.Executar(new FiltroFuncionarioDto() { CodigoDRE = DRE_CODIGO_1, NomeServidor = acessoAbae.Nome });

            funcionarios.ShouldNotBeNull();
            funcionarios.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "ABAE - Obter cadastros desconsiderando registros excluídos")]
        public async Task Obter_cadastros_desconsiderando_registros_excluidos()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia();
            var dre = new Dre() { CodigoDre = "1", DataAtualizacao = dataAtual };
            var ue = new Ue() { CodigoUe = "1", DreId = 1, DataAtualizacao = dataAtual };
            var cadAbae1 = new CadastroAcessoABAE()
            {
                UeId = 1,
                Nome = "cad1",
                Cpf = "1",
                Email = "@",
                Telefone = "1",
                Cep = "1",
                Endereco = "Rua",
                Numero = 1,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };
            var cadAbae2 = new CadastroAcessoABAE()
            {
                UeId = 1,
                Nome = "cad2",
                Cpf = "2",
                Email = "@",
                Telefone = "1",
                Cep = "1",
                Endereco = "Rua",
                Numero = 2,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "0",
                Excluido = true
            };

            CriarClaimUsuario(ObterPerfilDiretor());
            await CriarUsuarios();
            await InserirNaBase(dre);
            await InserirNaBase(ue);
            await InserirNaBase(cadAbae1);
            await InserirNaBase(cadAbae2);

            var useCase = ServiceProvider.GetService<IObterFuncionariosUseCase>();

            var resultado = await useCase.Executar(new FiltroFuncionarioDto()
            {
                CodigoDRE = "1",
                CodigoUE = "1"                
            });

            resultado.ShouldNotBeNull();
            resultado.ShouldNotContain(x => x.CodigoRf == cadAbae2.Cpf && x.NomeServidor == cadAbae2.Nome);
        }
    }
}
