using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse_PosConselho
{
    public class Ao_inserir_conceito_pos_conselho_bimestre : ConselhoDeClasseLancamentoBase
    {
        private const string JUSTIFICATIVA = "Nota pós conselho";

        public Ao_inserir_conceito_pos_conselho_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //protected override void RegistrarFakes(IServiceCollection services)
        //{
        //    base.RegistrarFakes(services);

        //}

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Deve_lancar_conceito_pos_conselho_bimestre(bool anoAnterior)
        {
            await CrieDados(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, ANO_4, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, anoAnterior);
            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Deve_lancar_conceito_pos_conselho_bimestre_regencia_fundamental(bool anoAnterior)
        {
            await CrieDados(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, TipoNota.Conceito, ANO_4, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, anoAnterior);
            await ExecuteTeste(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Deve_lancar_conceito_pos_conselho_bimestre_regencia_EJA(bool anoAnterior)
        {
            await CrieDados(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, ANO_4, Modalidade.EJA, ModalidadeTipoCalendario.EJA, anoAnterior);
            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        private async Task CrieDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior)
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
                DataAula = dataAula
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }

        private async Task ExecuteTeste(long componente, bool anoAnterior)
        {
            await ExecuteTeste(ObtenhaDto(componente), anoAnterior, ALUNO_CODIGO_1, TipoNota.Nota, BIMESTRE_2);
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
    }
}