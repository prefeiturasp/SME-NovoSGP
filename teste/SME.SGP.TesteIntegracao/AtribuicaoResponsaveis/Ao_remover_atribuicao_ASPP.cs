using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{

    public class Ao_remover_atribuicao_ASPP : TesteBase
    {
        const string DRE_CODIGO_1 = "1";
        const string DRE_CODIGO_2 = "2";
        const string SUPERVISOR_ID_1 = "1";
        const string SUPERVISOR_ID_2 = "2";
        const string SUPERVISOR_ID_3 = "3";
        const string SUPERVISOR_RF_01 = "1";
        const string SUPERVISOR_RF_02 = "2";
        const string SUPERVISOR_RF_03 = "3";
        public Ao_remover_atribuicao_ASPP(CollectionFixture collectionFixture) : base(collectionFixture) { }


        [Fact]
        public async Task Deve_Retornar_True()
        {
            //Arrange
            await InserirDre(DRE_CODIGO_1);
            await InserirSupervisorPsicologo("3", "3");
            await InserirSupervisorPsicoPedagogo("4", "4");
            await InserirSupervisorAssistenteSocial("5", "5");


            var _servicoEolFake = ServiceProvider.GetService<IServicoEol>();
            var useCase = ServiceProvider.GetService<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase>();
            var repositorio = ServiceProvider.GetService<IRepositorioSupervisorEscolaDre>();

            var listaAspp = new List<SupervisorEscolasDreDto>();

            //Act

            var retorno = await useCase.Executar(new MensagemRabbit(DRE_CODIGO_1));

            var registrosAposUseCase = ObterTodos<SupervisorEscolaDre>();
            var registrosDeletados = registrosAposUseCase.Count(x => x.Excluido);
            //Assert
            Assert.True(retorno);
            Assert.True(registrosDeletados > 0);
        }
        [Fact]
        public async Task Deve_retornar_false_quando_nao_passar_dre_valida()
        {
            //Arrange
            await InserirDre(DRE_CODIGO_2);
            
            var _servicoEolFake = ServiceProvider.GetService<IServicoEol>();
            var useCase = ServiceProvider.GetService<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase>();
            var repositorio = ServiceProvider.GetService<IRepositorioSupervisorEscolaDre>();

            //Act
            var retorno = await useCase.Executar(new MensagemRabbit(""));

            //Assert
            Assert.False(retorno);

        }

        [Fact]
        public async Task Deve_retornar_true_quando_nao_excluir_supervisores()
        {
            //Arrange
            await InserirDre(DRE_CODIGO_2);
            await InserirSupervisorPsicologo("10", "10");
            await InserirSupervisorPsicoPedagogo("9", "9");
            await InserirSupervisorAssistenteSocial("8", "8");

            var dre = "1";
            var _servicoEolFake = ServiceProvider.GetService<IServicoEol>();
            var useCase = ServiceProvider.GetService<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase>();
            var repositorio = ServiceProvider.GetService<IRepositorioSupervisorEscolaDre>();
            var registrosAntesUseCase = ObterTodos<SupervisorEscolaDre>();

            //Act
            var retorno = await useCase.Executar(new MensagemRabbit(dre));
            var registrosAposUseCase = ObterTodos<SupervisorEscolaDre>();
            //Assert
            Assert.True(registrosAntesUseCase.Count() == registrosAposUseCase.Count());

        }

        public async Task InserirDre(string codigoDre)
        {
            await InserirNaBase(new Dre()
            {
                Abreviacao = "DT",
                CodigoDre = codigoDre,
                DataAtualizacao = DateTime.Now,
                Nome = "Dre Teste"
            });
        }



        public async Task InserirSupervisorPsicologo(string id, string rf)
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                DreId = "1",
                EscolaId = "1",
                SupervisorId = id,
                Tipo = 3,
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = rf,
                Excluido = false
            });

        }
        public async Task InserirSupervisorPsicoPedagogo(string id, string rf)
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                DreId = "1",
                EscolaId = "1",
                SupervisorId = id,
                Tipo = 4,
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = rf,
                Excluido = false
            });

        }
        public async Task InserirSupervisorAssistenteSocial(string id, string rf)
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                DreId = "1",
                EscolaId = "1",
                SupervisorId = id,
                Tipo = 5,
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = rf,
                Excluido = false
            });

        }
    }
}
