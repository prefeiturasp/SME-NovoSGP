using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtribuicaoResponsavel
{
    public class Ao_remover_atribuicao_aspp : AtribuicaoResponsavelTesteBase
    {
        #region Constants
        private const string DRE_CODIGO_1 = "1";
        private const string DRE_CODIGO_2 = "2";
        private const string PSICOLOGO_RESPONSAVEL_ID = "3";
        private const string PSICOLOGO_RESPONSAVEL_RF_ID = "3";
        private const string PSICOLOGO_RESPONSAVEL_ID_2 = "10";
        private const string PSICOLOGO_RESPONSAVEL_RF_ID_2 = "10";
        private const string PSICOPEDAGOGO_RESPONSAVEL_RF_ID = "4";
        private const string PSICOPEDAGOGO_RESPONSAVEL_ID = "4";
        private const string PSICOPEDAGOGO_RESPONSAVEL_RF_ID_2 = "9";
        private const string PSICOPEDAGOGO_RESPONSAVEL_ID_2 = "9";
        private const string ASSISTENTE_SOCIAL_RESPONSAVEL_ID = "5";
        private const string ASSISTENTE_SOCIAL_RESPONSAVEL_RF_ID = "5";
        private const string ASSISTENTE_SOCIAL_RESPONSAVEL_ID_2 = "6";
        private const string ASSISTENTE_SOCIAL_RESPONSAVEL_RF_ID_2 = "6";
        private const string ASSISTENTE_SOCIAL_RESPONSAVEL_ID_3 = "8";
        private const string ASSISTENTE_SOCIAL_RESPONSAVEL_RF_ID_3 = "8";
        #endregion

        public Ao_remover_atribuicao_aspp(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_retornar_true_quando_exlcuir_alguns_responsaveis_mas_nao_todos()
        {
            //Arrange
            await InserirDre(DRE_CODIGO_1);
            await InserirResponsavelPsicologo(PSICOLOGO_RESPONSAVEL_ID, PSICOLOGO_RESPONSAVEL_RF_ID);
            await InserirResponsavelPsicoPedagogo(PSICOPEDAGOGO_RESPONSAVEL_ID, PSICOPEDAGOGO_RESPONSAVEL_RF_ID);
            await InserirResponsavelAssistenteSocial(ASSISTENTE_SOCIAL_RESPONSAVEL_ID, ASSISTENTE_SOCIAL_RESPONSAVEL_RF_ID);
            await InserirResponsavelAssistenteSocial(ASSISTENTE_SOCIAL_RESPONSAVEL_ID_2, ASSISTENTE_SOCIAL_RESPONSAVEL_RF_ID_2);

            var useCase = ServiceProvider.GetService<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase>();
            var listaAspp = new List<SupervisorEscolasDreDto>();
            //Act
            var retorno = await useCase.Executar(new MensagemRabbit(DRE_CODIGO_1));
            var registrosAposUseCase = ObterTodos<SupervisorEscolaDre>();
            //Assert
            Assert.True(retorno);
            Assert.True(registrosAposUseCase.Count(x => x.Excluido) == 1);
            Assert.True(registrosAposUseCase.Count(x => !x.Excluido) == 3);
        }

        [Fact]
        public async Task Deve_retornar_true_quando_excluir_responsaveis_nao_estao_no_eol()
        {
            //Arrange
            await InserirDre(DRE_CODIGO_1);
            await InserirResponsavelPsicologo(PSICOLOGO_RESPONSAVEL_ID_2, PSICOLOGO_RESPONSAVEL_RF_ID_2);
            await InserirResponsavelPsicoPedagogo(PSICOPEDAGOGO_RESPONSAVEL_ID_2, PSICOPEDAGOGO_RESPONSAVEL_RF_ID_2);
            await InserirResponsavelAssistenteSocial(ASSISTENTE_SOCIAL_RESPONSAVEL_ID_3, ASSISTENTE_SOCIAL_RESPONSAVEL_RF_ID_3);

            var useCase = ServiceProvider.GetService<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase>();
            var repositorio = ServiceProvider.GetService<IRepositorioSupervisorEscolaDre>();
            var registrosAntesUseCase = ObterTodos<SupervisorEscolaDre>();

            //Act
            var retorno = await useCase.Executar(new MensagemRabbit(DRE_CODIGO_1));
            var registrosAposUseCase = ObterTodos<SupervisorEscolaDre>();
            //Assert
            Assert.True(registrosAposUseCase.Count(x => x.Excluido) == 3);
        }

        [Fact]
        public async Task Deve_retornar_true_quando_nao_excluir_responsaveis()
        {
            //Arrange
            await InserirDre(DRE_CODIGO_1);
            await InserirResponsavelPsicologo(PSICOLOGO_RESPONSAVEL_ID, PSICOLOGO_RESPONSAVEL_RF_ID);
            await InserirResponsavelPsicoPedagogo(PSICOPEDAGOGO_RESPONSAVEL_ID, PSICOPEDAGOGO_RESPONSAVEL_RF_ID);
            await InserirResponsavelAssistenteSocial(ASSISTENTE_SOCIAL_RESPONSAVEL_ID, ASSISTENTE_SOCIAL_RESPONSAVEL_RF_ID);

            var useCase = ServiceProvider.GetService<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase>();
            var repositorio = ServiceProvider.GetService<IRepositorioSupervisorEscolaDre>();
            var registrosAntesUseCase = ObterTodos<SupervisorEscolaDre>();

            //Act
            var retorno = await useCase.Executar(new MensagemRabbit(DRE_CODIGO_1));
            var registrosAposUseCase = ObterTodos<SupervisorEscolaDre>();
            //Assert
            Assert.True(registrosAposUseCase.Count(x => x.Excluido) == 0);
        }

        [Fact]
        public async Task Deve_retornar_true_quando_nao_excluir_nada_no_SGP()
        {
            //Arrange
            await InserirDre(DRE_CODIGO_1);
            var useCase = ServiceProvider.GetService<IRemoverAtribuicaoResponsaveisASPPPorDreUseCase>();

            //Act
            var retorno = await useCase.Executar(new MensagemRabbit(DRE_CODIGO_1));
            //Assert
            Assert.True(retorno);
        }

        #region Cargas
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

        public async Task InserirResponsavelPsicologo(string id, string rf)
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

        public async Task InserirResponsavelPsicoPedagogo(string id, string rf)
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

        public async Task InserirResponsavelAssistenteSocial(string id, string rf)
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
        #endregion
    }
}