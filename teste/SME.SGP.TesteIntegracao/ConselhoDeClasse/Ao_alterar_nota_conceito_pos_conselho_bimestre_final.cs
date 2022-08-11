using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_alterar_nota_conceito_pos_conselho_bimestre_final : ConselhoDeClasseTesteBase
    {
        public Ao_alterar_nota_conceito_pos_conselho_bimestre_final(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Theory]
        [InlineData(false, BIMESTRE_2)]
        [InlineData(false, BIMESTRE_FINAL)]
        [InlineData(true, BIMESTRE_2)]
        [InlineData(true, BIMESTRE_FINAL)]
        public async Task Ao_alterar_nota_conceito_pos_conselho_bimestre_e_final_fundamental(bool anoAnterior, int bimestre)
        {
            await CrieDados(
                ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior,
                bimestre);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 0, (int)ConceitoValores.NS, anoAnterior);
            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, CONSELHO_CLASSE_ID_1, (int)ConceitoValores.P, anoAnterior);
        }

        [Theory]
        [InlineData(false, BIMESTRE_2)]
        [InlineData(false, BIMESTRE_FINAL)]
        [InlineData(true, BIMESTRE_2)]
        [InlineData(true, BIMESTRE_FINAL)]
        public async Task Ao_alterar_nota_conceito_pos_conselho_bimestre_e_final_eja(bool anoAnterior, int bimestre)
        {
            await CrieDados(
                ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ANO_9,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                anoAnterior,
                bimestre);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 0, (int)ConceitoValores.NS, anoAnterior);
            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, CONSELHO_CLASSE_ID_1, (int)ConceitoValores.P, anoAnterior);
        }

        [Theory]
        [InlineData(false, BIMESTRE_2)]
        [InlineData(false, BIMESTRE_FINAL)]
        [InlineData(true, BIMESTRE_2)]
        [InlineData(true, BIMESTRE_FINAL)]
        public async Task Ao_alterar_nota_conceito_pos_conselho_bimestre_e_final_regencia_classe(bool anoAnterior, int bimestre)
        {
            await CrieDados(
                ObterPerfilProfessor(),
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior,
                bimestre);

            await ExecuteTeste(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, 0, (int)ConceitoValores.NS, anoAnterior);
            await ExecuteTeste(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, CONSELHO_CLASSE_ID_1, (int)ConceitoValores.P, anoAnterior);
        }

        private async Task ExecuteTeste(long componente, int conceitoClasseId, long conceito, bool anoAnterior)
        {
            await ExecutarTeste(
                        ObtenhaDto(componente, conceito),
                        conceitoClasseId,
                        anoAnterior,
                        ALUNO_CODIGO_1,
                        TipoNota.Nota,
                        BIMESTRE_FINAL);
        }

        private ConselhoClasseNotaDto ObtenhaDto(long componente, long conceito)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componente,
                Conceito = conceito,
                Justificativa = JUSTIFICATIVA
            };
        }

        private async Task CrieDados(
                string perfil,
                long componente,
                string anoTurma,
                Modalidade modalidade,
                ModalidadeTipoCalendario modalidadeTipoCalendario,
                bool anoAnterior,
                int bimestre)
        {
            DateTime dataAula;

            if (bimestre == BIMESTRE_FINAL)
                dataAula = anoAnterior ? DATA_03_10_INICIO_BIMESTRE_4.AddYears(-1) : DATA_03_10_INICIO_BIMESTRE_4;
            else
                dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = bimestre,
                ComponenteCurricular = componente.ToString(),
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula
            };

            await CriarDadosBase(filtroNota);
        }
    }
}
