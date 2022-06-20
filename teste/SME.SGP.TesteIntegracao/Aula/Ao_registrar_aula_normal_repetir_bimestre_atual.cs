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
        public Ao_registrar_aula_normal_repetir_bimestre_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_com_atribuicao_cj_e_com_atribuicao_espontanea_invalida_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await CriarAtribuicaoCJ(Modalidade.Fundamental);

            await CriarAtribuicaoEsporadica(new DateTime(2022, 2, 15), new DateTime(2022, 2, 15));

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente.");
        }        

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_modalidade_infantil()
        {
            await CriarDadosBasicosAula(ObterPerfilCJInfantil(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);

        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_escolar_cp_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCP(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_escolar_ad_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilAD(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilDiretor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }
    }
}
