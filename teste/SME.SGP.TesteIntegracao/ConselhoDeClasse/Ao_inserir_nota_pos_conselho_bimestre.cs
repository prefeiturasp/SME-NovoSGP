using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_nota_pos_conselho_bimestre : ConselhoDeClasseTesteBase
    {
        public Ao_inserir_nota_pos_conselho_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_fundamental(bool anoAnterior) 
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                ANO_7, 
                Modalidade.Fundamental, 
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_fundamental_cp(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilCP(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ANO_5,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, false);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_medio(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                ANO_7, 
                Modalidade.Medio, 
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_medio_diretor(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilDiretor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ANO_5,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, false);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_eja(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                ANO_9, 
                Modalidade.EJA, 
                ModalidadeTipoCalendario.EJA,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_regencia_classe(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, 
                ANO_1, 
                Modalidade.Fundamental, 
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, anoAnterior);
        }

        private async Task ExecuteTeste(long componente, bool anoAnterior)
        {
            await ExecuteTeste(
                        ObtenhaDto(componente),
                        0,
                        anoAnterior,
                        ALUNO_CODIGO_1,
                        TipoNota.Nota,
                        BIMESTRE_2);
        }

        private ConselhoClasseNotaDto ObtenhaDto(long componente)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componente,
                Nota = NOTA_7,
                Justificativa = JUSTIFICATIVA
            };
        }

        private async Task CrieDados(
                        string perfil, 
                        long componente,
                        string anoTurma, 
                        Modalidade modalidade,
                        ModalidadeTipoCalendario modalidadeTipoCalendario,
                        bool anoAnterior)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula
            };
            
            await CriarDadosBase(filtroNota);
        }
    }
}
