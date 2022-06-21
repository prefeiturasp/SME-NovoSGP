using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual_excecoes_basicas : AulaTeste
    {
        private const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        private const long COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999 = 999999;
        private DateTime dataInicio = new DateTime(2022, 05, 02);
        private DateTime dataFim = new DateTime(2022, 07, 08);

        public Ao_registrar_aula_normal_repetir_bimestre_atual_excecoes_basicas(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_com_atribuicao_cj_e_com_atribuicao_espontanea_invalida_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarAtribuicaoEsporadica(new DateTime(2022, 2, 15), new DateTime(2022, 2, 15));

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Você não pode criar aulas para essa Turma.");
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_cj_com_atribuicao_cj_e_com_atribuicao_espontanea_valida_componentes_invalidos_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, false);

            await CriarAtribuicaoEsporadica(dataInicio, dataInicio);

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Você não pode criar aulas para essa Turma.");
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_nao_pode_cadastrar_aula_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999, dataInicio);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Você não pode criar aulas para essa Turma.");
        }
            
    }
}