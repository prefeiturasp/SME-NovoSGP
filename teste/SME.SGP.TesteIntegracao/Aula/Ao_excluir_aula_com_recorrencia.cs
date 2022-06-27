using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaRecorrencia
{
    public class Ao_excluir_aula_com_recorrencia : AulaTeste
    {
        private DateTime dataInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 02);

        public Ao_excluir_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Excluir_aula_com_recorrencia_no_bimestre()
        {
            await Excluir_aula_com_regencia(RecorrenciaAula.RepetirBimestreAtual);
        }

        [Fact]
        public async Task Excluir_aula_com_recorrencia_em_todos_os_bimestres()
        {
            await Excluir_aula_com_regencia(RecorrenciaAula.RepetirTodosBimestres);
        }

        [Fact]
        public async Task Aula_com_avaliacao_vinculada()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.RepetirBimestreAtual);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);
            await CriarAtividadeAvaliativaFundamental(dataInicio);

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObterExcluirAulaDto(RecorrenciaAula.RepetirBimestreAtual);
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Notificacao>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Mensagem.ShouldContain("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");
        }

        protected async Task Excluir_aula_com_regencia(RecorrenciaAula recorrencia)
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, recorrencia);
            await CriaAulaRecorrentePortugues(recorrencia);

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObterExcluirAulaDto(recorrencia);

            await CriarPeriodoEscolarEAbertura();

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Excluido.ShouldBe(true);
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
