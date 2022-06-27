using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaUnica
{
    public class Ao_registrar_aula_professor_cj : AulaTeste
    {
        private DateTime dataInicio = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime dataFim = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_registrar_aula_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_CJ()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
        }

        [Fact]
        public async Task Professor_CJ_sem_permissao_para_cadastrar_atribuicao_encerrada()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarAtribuicaoEsporadica(new(DateTimeExtension.HorarioBrasilia().Year, 01, 10), new(DateTimeExtension.HorarioBrasilia().Year, 01, 10));

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não possui permissão para cadastrar aulas neste período");
        }

        [Fact]
        public async Task Professor_CJ_nao_pode_criar_aulas()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não pode criar aulas para essa Turma.");
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_data_com_evento_nao_letivo()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarEvento(EventoLetivo.Nao, dataInicio, dataInicio);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        }

        [Fact]
        public async Task Cadastrar_aula_para_regencia_de_classe_Fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_1);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, dataInicio, true);
        }

        [Fact]
        public async Task Cadastrar_aula_para_regencia_de_classe_EJA()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.EJA, ModalidadeTipoCalendario.EJA, dataInicio, dataFim, BIMESTRE_1);
            await CriarAtribuicaoCJ(Modalidade.EJA, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113, dataInicio, true);
        }

        [Fact]
        public async Task Cadastrar_aula_para_componente_nao_regencia()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.EJA, ModalidadeTipoCalendario.EJA, dataInicio, dataFim, BIMESTRE_1);
            await CriarAtribuicaoCJ(Modalidade.Medio, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio, false);
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