using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual_excecoes_basicas : AulaTeste
    {
        private readonly DateTime DATA_15_02 = new(DateTimeExtension.HorarioBrasilia().Year, 02, 15);

        public Ao_registrar_aula_normal_repetir_bimestre_atual_excecoes_basicas(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_com_atribuicao_cj_e_com_atribuicao_espontanea_invalida_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2,false);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarAtribuicaoEsporadica(DATA_15_02, DATA_15_02);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo($"Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: {MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma}");
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_cj_com_atribuicao_cj_e_com_atribuicao_espontanea_valida_componentes_invalidos_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, false);

            await CriarAtribuicaoEsporadica(DATA_02_05, DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_ARTES_ID_139, DATA_02_05);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo($"Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: {MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma}");
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_nao_pode_cadastrar_aula_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999, DATA_02_05);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo($"Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: {MensagemNegocioComuns.Voce_nao_pode_criar_aulas_para_essa_turma}");
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

    }
}