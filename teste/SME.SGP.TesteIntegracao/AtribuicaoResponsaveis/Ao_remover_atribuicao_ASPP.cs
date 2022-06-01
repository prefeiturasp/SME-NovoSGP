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
