using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_excluir_compensacao_de_ausencia : CompensacaoDeAusenciaTesteBase
    {
        private const int COMPENSACAO_AUSENCIA_ID_1 = 1;
        public Ao_excluir_compensacao_de_ausencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        public async Task Deve_excluir_compensacao_pelo_professor_titular()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaCompensacaoAusencia(dtoDadoBase);
            await CriaCompensacaoAusenciaAluno();

            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var listaIds = new long[] { COMPENSACAO_AUSENCIA_ID_1 };

            await comando.Excluir(listaIds);
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

        private async Task CriaCompensacaoAusenciaAluno(string codigoAluno, int quantidadeCompensada)
        {
            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                CodigoAluno = codigoAluno,
                CompensacaoAusenciaId = COMPENSACAO_AUSENCIA_ID_1,
                QuantidadeFaltasCompensadas = quantidadeCompensada,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_1,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_2,
            QUANTIDADE_AULA,
            QUANTIDADE_AULA_3);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_3,
            QUANTIDADE_AULA_2,
            QUANTIDADE_AULA_2);

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_4,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA);
        }

        private async Task CriaFrequenciaAlunos(
                CompensacaoDeAusenciaDBDto dtoDadoBase,
                string codigoAluno,
                int totalPresenca,
                int totalAusencia)
        {
            await CriaFrequenciaAluno(
                dtoDadoBase,
                DATA_03_01_INICIO_BIMESTRE_1,
                DATA_29_04_FIM_BIMESTRE_1,
                codigoAluno,
                totalPresenca,
                totalAusencia,
                PERIODO_ESCOLAR_ID_1);
        }

        private CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }
    }
}
