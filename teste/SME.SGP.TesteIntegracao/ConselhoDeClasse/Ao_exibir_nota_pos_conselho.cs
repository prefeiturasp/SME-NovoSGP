using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_exibir_nota_pos_conselho : ConselhoDeClasseTesteBase
    {
        public Ao_exibir_nota_pos_conselho(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        public async Task Deve_exibir_na_tela_de_conselho_notas_lancadas_na_tela_de_conselho(bool anoAnterior = false)
        {
            await CriarDados(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, ANO_8, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, anoAnterior, SituacaoConselhoClasse.EmAndamento, true);

        }
        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }

        private ConselhoClasseNotaDto ObterConselhoClasseNotaDto(long componente)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componente,
                Nota = NOTA_7,
                Justificativa = JUSTIFICATIVA
            };
        }


    }
}