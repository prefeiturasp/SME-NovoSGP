using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual : AulaTeste
    {
        public Ao_registrar_aula_normal_repetir_bimestre_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        //[Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_com_atribuicao_cj_e_sem_atribuicao_espontanea_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await CriarAtribuicaoCJ(Modalidade.Fundamental);

            await CriarAtribuicaoEsporadica(new DateTime(2022, 01, 10), new DateTime(2022, 01, 10));

            var retorno = await ValidarInserirAulaUseCaseBasico(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);

        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_cj_modalidade_infantil()
        {
            await CriarDadosBasicosAula(ObterPerfilCJInfantil(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil);

            var retorno = await ValidarInserirAulaUseCaseBasico(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);

        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_professor_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await ValidarInserirAulaUseCaseBasico(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_escolar_cp_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCP(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await ValidarInserirAulaUseCaseBasico(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_escolar_ad_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilAD(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await ValidarInserirAulaUseCaseBasico(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }

        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_como_diretor_modalidade_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilDiretor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var retorno = await ValidarInserirAulaUseCaseBasico(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
        }
    }
}
