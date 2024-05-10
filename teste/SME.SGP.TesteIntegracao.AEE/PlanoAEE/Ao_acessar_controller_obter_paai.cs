using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_acessar_controller_obter_paai : TesteBase
    {
        const string DRE_CODIGO_1 = "1";
        const string SUPERVISOR_ID_2 = "2";
        const string SUPERVISOR_RF_03 = "3";
        const string DRE_TESTE = "DT";
        const string DRE_NOME = "DRE TESTE";
        const string ESCOLA_ID = "1";
        const string CRIADO_POR = "TESTE";
        public Ao_acessar_controller_obter_paai(CollectionFixture collectionFixture) : base(collectionFixture) { }

        [Fact(DisplayName = "Plano AEE - Deve retornar verdadeiro quando houver registros na lista de paai")]
        public async Task Deve_retornar_true_quando_houver_registros_na_lista_de_paai()
        {
            await InserirDre(DRE_CODIGO_1);
            await InserirSupervisorPAAI(SUPERVISOR_ID_2, SUPERVISOR_RF_03);

            var useCase = ServiceProvider.GetService<IObterPAAIPorDreUseCase>();
            var retorno = await useCase.Executar(DRE_CODIGO_1);

            Assert.True(retorno.Count() > 0);
        }
        [Fact(DisplayName = "Plano AEE - Deve retornar verdadeiro quando não houver registros na lista de paai")]
        public async Task Deve_retornar_true_quando_nao_houver_registros_na_lista_de_paai()
        {
            await InserirDre(DRE_CODIGO_1);

            var useCase = ServiceProvider.GetService<IObterPAAIPorDreUseCase>();
            var retorno = await useCase.Executar(DRE_CODIGO_1);

            Assert.True(retorno.Count() == 0);
        }

        public async Task InserirDre(string codigoDre)
        {
            await InserirNaBase(new Dre()
            {
                Abreviacao = DRE_TESTE,
                CodigoDre = codigoDre,
                DataAtualizacao = DateTime.Now,
                Nome = DRE_NOME
            });
        }


        public async Task InserirSupervisorPAAI(string id, string rf)
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                DreId = DRE_CODIGO_1,
                EscolaId = ESCOLA_ID,
                SupervisorId = id,
                Tipo = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = CRIADO_POR,
                CriadoRF = rf,
                Excluido = false
            });

        }
    }
}
