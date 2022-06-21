using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaUnica
{
    public class Ao_registrar_aula_professor_cj : AulaTeste
    {
        private const long COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213 = 1213;
        private const long COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1213 = 1113;
        private DateTime DATA_10_02_2022 = new DateTime(2022, 02, 10);

        public Ao_registrar_aula_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_CJ()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));
        }

        [Fact]
        public async Task Professor_CJ_sem_permissao_para_cadastrar_atribuicao_encerrada()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarAtribuicaoEsporadica(new DateTime(2022, 01, 10), new DateTime(2022, 01, 10));

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não possui permissão para cadastrar aulas neste período");
        }

        [Fact]
        public async Task Professor_CJ_nao_pode_criar_aulas()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não pode criar aulas para essa Turma.");
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_data_com_evento_nao_letivo()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CrieEvento(EventoLetivo.Nao, new System.DateTime(2022, 02, 10), new System.DateTime(2022, 02, 10));

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        }

        [Fact]
        public async Task Cadastrar_aula_para_regencia_de_classe_Fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            var excecao = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_10_02_2022, true);
        }

        [Fact]
        public async Task Cadastrar_aula_para_regencia_de_classe_EJA()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.EJA, ModalidadeTipoCalendario.EJA);
            await CriarAtribuicaoCJ(Modalidade.EJA, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1213);

            var excecao = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1213, DATA_10_02_2022, true);
        }

        [Fact]
        public async Task Cadastrar_aula_para_componente_nao_regencia()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.EJA, ModalidadeTipoCalendario.EJA);
            await CriarAtribuicaoCJ(Modalidade.Medio, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            var excecao = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_10_02_2022, false);
        }
    }
}