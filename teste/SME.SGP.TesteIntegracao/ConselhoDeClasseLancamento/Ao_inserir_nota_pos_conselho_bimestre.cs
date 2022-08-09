using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento
{
    public class Ao_inserir_nota_pos_conselho_bimestre : ConselhoDeClasseLancamentoBase
    {
        private const string JUSTIFICATIVA = "Nota pós conselho";
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

            await ExecuteTeste(ObtenhaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), anoAnterior, ALUNO_CODIGO_1, TipoNota.Nota);
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

            await ExecuteTeste(ObtenhaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), anoAnterior, ALUNO_CODIGO_1, TipoNota.Nota);
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

            await ExecuteTeste(ObtenhaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), anoAnterior, ALUNO_CODIGO_1, TipoNota.Nota);
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

            await ExecuteTeste(ObtenhaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105), anoAnterior, ALUNO_CODIGO_1, TipoNota.Nota);
        }


        private ConselhoClasseNotaDto ObtenhaDto(long componente)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componente,
                Nota = 7,
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
