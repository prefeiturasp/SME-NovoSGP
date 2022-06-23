using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaRecorrencia
{
    public class Ao_alterar_aula_com_recorrencia : AulaTeste
    {
        private DateTime DATA_INICIO = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime DATA_FIM = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_alterar_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture) { }

        [Fact]
        public async Task Altera_quantidade_de_aulas_com_recorrencia_no_bimestre_atual()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_INICIO, DATA_FIM, BIMESTRE_2);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var aula = ObterAula(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, 138, DATA_INICIO);
            aula.DataAula = new DateTime(2022, 06, 27);

            await CriarPeriodoEscolarEAbertura();

            var retorno = await usecase.Executar(aula);
            var listaNotificao = ObterTodos<Notificacao>();

            retorno.ShouldNotBeNull();
            listaNotificao.FirstOrDefault().Mensagem.ShouldContain("Foram alteradas 2 aulas do componente curricular Português para a turma Turma Nome 1 da Nome da UE (DRE 1).");
        }

        [Fact]
        public async Task Altera_quantidade_de_aulas_com_recorrenciapara_todos_bimestres()
        {

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_INICIO, DATA_FIM, BIMESTRE_1, false);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirTodosBimestres);

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var aula = ObterAula(TipoAula.Normal, RecorrenciaAula.RepetirTodosBimestres, 138, DATA_INICIO);
            aula.DataAula = new DateTime(2022, 08, 26);

            await CriarPeriodoEscolarEAbertura();

            var retorno = await usecase.Executar(aula);
            var listaNotificao = ObterTodos<Notificacao>();

            retorno.ShouldNotBeNull();
            listaNotificao.ShouldNotBeEmpty();
            listaNotificao.FirstOrDefault().Mensagem.ShouldContain("Foram alteradas 17 aulas do componente curricular Português para a turma Turma Nome 1 da Nome da UE (DRE 1).");

        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_1, DATA_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_2, DATA_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3, DATA_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_4, DATA_FIM_BIMESTRE_4, BIMESTRE_4);
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}
