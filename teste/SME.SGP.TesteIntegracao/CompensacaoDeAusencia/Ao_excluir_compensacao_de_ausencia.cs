using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_excluir_compensacao_de_ausencia : CompensacaoDeAusenciaTesteBase
    {
        public Ao_excluir_compensacao_de_ausencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_excluir_compensacao_pelo_professor_titular()
        {
            await ExecuteTesteDeExclusao(ObterPerfilProfessor());
        }

        [Fact]
        public async Task Deve_excluir_compensacao_pelo_cj()
        {
            await ExecuteTesteDeExclusao(ObterPerfilCJ());
        }

        [Fact]
        public async Task Deve_excluir_compensacao_pelo_cp()
        {
            await ExecuteTesteDeExclusao(ObterPerfilCP());
        }

        [Fact]
        public async Task Deve_excluir_compensacao_pelo_diretor()
        {
            await ExecuteTesteDeExclusao(ObterPerfilDiretor());
        }

        private async Task ExecuteTesteDeExclusao(string perfil)
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(perfil, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaCompensacaoAusencia(dtoDadoBase);
            await CriaCompensacaoAusenciaAluno();
            await CriaRegistroDeFrequencia();

            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var listaIds = new long[] { COMPENSACAO_AUSENCIA_ID_1 };

            await comando.Excluir(listaIds);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            listaDeCompensacaoAusencia.FirstOrDefault().Excluido.ShouldBeTrue();
            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            listaDeCompensacaoAusenciaAluno.ForEach(ausencia => ausencia.Excluido.ShouldBeTrue());
            var listaDeFrequenciaAluno = ObterTodos<Dominio.FrequenciaAluno>();
            listaDeFrequenciaAluno.ShouldNotBeNull();
            listaDeFrequenciaAluno.ForEach(frequencia => frequencia.TotalCompensacoes.ShouldBe(0));
        }

        private async Task CriaCompensacaoAusenciaAluno()
        {
            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_1,
                    QUANTIDADE_AULA);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_2,
                    QUANTIDADE_AULA_3);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_3,
                    QUANTIDADE_AULA_2);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_4,
                    QUANTIDADE_AULA);
        }

        private async Task CriaRegistroDeFrequencia()
        {
            await CrieRegistroDeFrenquencia();
            await RegistroFrequenciaAluno(CODIGO_ALUNO_1, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_2, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_3, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_4, QUANTIDADE_AULA, TipoFrequencia.F);
        }

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_1,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_2,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA_2);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_3,
            QUANTIDADE_AULA_2,
            QUANTIDADE_AULA_2,
            QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_4,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA);
        }

        private async Task CriaFrequenciaAlunos(
                CompensacaoDeAusenciaDBDto dtoDadoBase,
                string codigoAluno,
                int totalPresenca,
                int totalAusencia,
                int totalCompensacao)
        {
            await CriaFrequenciaAluno(
                dtoDadoBase,
                DATA_25_07_INICIO_BIMESTRE_3,
                DATA_30_09_FIM_BIMESTRE_3,
                codigoAluno,
                totalPresenca,
                totalAusencia,
                PERIODO_ESCOLAR_ID_3,
                totalCompensacao);
        }

        private CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_3,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_30_09_FIM_BIMESTRE_3,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }
    }
}
