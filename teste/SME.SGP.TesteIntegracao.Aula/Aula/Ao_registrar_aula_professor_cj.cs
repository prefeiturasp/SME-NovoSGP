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
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaUnica
{
    public class Ao_registrar_aula_professor_cj : AulaTeste
    {
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
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
        }

        [Fact]
        public async Task Professor_CJ_sem_permissao_para_cadastrar_atribuicao_encerrada()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CriarAtribuicaoEsporadica(new(DateTimeExtension.HorarioBrasilia().Year, 01, 10), new(DateTimeExtension.HorarioBrasilia().Year, 01, 10));

            await CriarPeriodoEscolarEAbertura();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não possui permissão para cadastrar aulas neste período");
        }

        [Fact]
        public async Task Professor_CJ_nao_pode_criar_aulas()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999, DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe(MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_data_com_evento_nao_letivo()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarEvento(EventoLetivo.Nao, DATA_02_05, DATA_02_05);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        }

        [Fact]
        public async Task Cadastrar_aula_para_regencia_de_classe_Fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_1, false);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_02_05, true);
        }

        [Fact]
        public async Task Cadastrar_aula_para_regencia_de_classe_EJA()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.EJA, ModalidadeTipoCalendario.EJA, DATA_02_05, DATA_07_08, BIMESTRE_1, false);

            await CriarAtribuicaoCJ(Modalidade.EJA, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113, DATA_02_05, true);
        }

        [Fact]
        public async Task Cadastrar_aula_para_componente_nao_regencia()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.EJA, ModalidadeTipoCalendario.EJA, DATA_02_05, DATA_07_08, BIMESTRE_1, false);

            await CriarAtribuicaoCJ(Modalidade.Medio, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05, false);
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}