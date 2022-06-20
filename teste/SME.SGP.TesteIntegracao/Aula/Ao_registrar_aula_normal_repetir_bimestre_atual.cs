using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual : AulaTeste
    {
        private const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        private const long COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999 = 999999;

        public Ao_registrar_aula_normal_repetir_bimestre_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_com_atribuicao_cj_e_com_atribuicao_espontanea_invalida_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarAtribuicaoEsporadica(new DateTime(2022, 2, 15), new DateTime(2022, 2, 15));

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new DateTime(2022,02,10));

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Você não pode criar aulas para essa Turma.");
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_cj_com_atribuicao_cj_e_com_atribuicao_espontanea_valida_componentes_invalidos_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138,false);

            await CriarAtribuicaoEsporadica(new DateTime(2022, 2, 10), new DateTime(2022, 2, 10));

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new DateTime(2022, 02, 10));

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Você não pode criar aulas para essa Turma.");
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_nao_pode_cadastrar_aula_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999, new DateTime(2022, 02, 10));

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Você não pode criar aulas para essa Turma.");
        }





















        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_modalidade_infantil()
        {
            await CriarDadosBasicosAula(ObterPerfilCJInfantil(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new DateTime(2022,02,10));

        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_escolar_cp_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCP(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_escolar_ad_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilAD(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilDiretor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new System.DateTime(2022, 02, 10));
        }
    }
}
