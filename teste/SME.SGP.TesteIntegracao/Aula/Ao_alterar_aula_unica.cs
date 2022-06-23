using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System;

namespace SME.SGP.TesteIntegracao.TestarAulaUnica
{
    public class Ao_alterar_aula_unica : AulaTeste
    {
        private DateTime dataInicio = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime dataFim = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_alterar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ja_existe_aula_criada_no_dia_para_o_componente()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
            dto.Id = 2;

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Já existe uma aula criada neste dia para este componente curricular");
        }

        [Fact]
        public async Task Nao_e_possivel_alterar_aula_fora_do_periodo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2, false);
            await CriarPeriodoEscolarEncerrado();
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
            dto.Id = 1;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula fora do periodo escolar");
        }

        [Fact]
        public async Task Altera_aula()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
            dto.Id = 1;
            dto.Quantidade = 1;

            await CriarPeriodoEscolarEAbertura();

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Quantidade.ShouldBe(1);
        }

        [Fact]
        public async Task Altera_aula_regente_diferente_do_atual()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.AulaUnica, "1111111");

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
            dto.Id = 1;
            dto.Quantidade = 1;
            dto.EhRegencia = true;

            await CriarPeriodoEscolarEAbertura();
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Quantidade.ShouldBe(1);
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
