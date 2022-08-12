using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_nota_numerica_pos_conselho_final : ConselhoDeClasseTesteBase
    {
        public Ao_inserir_nota_numerica_pos_conselho_final(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_fundamental(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Nota);
            salvarConselhoClasseAlunoNotaDto.Bimestre = BIMESTRE_FINAL;
            
            await CrieDados(ObterPerfilProfessor(),
                            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                            ANO_7,
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            anoAnterior);

            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_fundamental_cp(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Nota);
            salvarConselhoClasseAlunoNotaDto.Bimestre = BIMESTRE_FINAL;
            
            await CrieDados(
                ObterPerfilCP(),
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                ANO_5,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselho_bimestre_numerica_medio(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Nota);
            salvarConselhoClasseAlunoNotaDto.Bimestre = BIMESTRE_FINAL;
            
            await CrieDados(ObterPerfilProfessor(),
                            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                            ANO_7,
                            Modalidade.Medio,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            anoAnterior);

            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_medio_diretor(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Nota);
            salvarConselhoClasseAlunoNotaDto.Bimestre = BIMESTRE_FINAL;
            
            await CrieDados(
                ObterPerfilDiretor(),
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                ANO_5,
                Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_eja(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Nota);
            salvarConselhoClasseAlunoNotaDto.Bimestre = BIMESTRE_FINAL;
            
            await CrieDados(
                ObterPerfilProfessor(),
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                ANO_9,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                anoAnterior);

            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselho_bimestre_numerica_regencia_classe(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Nota);
            salvarConselhoClasseAlunoNotaDto.Bimestre = BIMESTRE_FINAL;
            
            await CrieDados(
                ObterPerfilProfessor(),
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        private async Task CrieDados(
                        string perfil,
                        long componente,
                        string anoTurma,
                        Modalidade modalidade,
                        ModalidadeTipoCalendario modalidadeTipoCalendario,
                        bool anoAnterior)
        {
            var dataAula = anoAnterior ? DATA_03_10_INICIO_BIMESTRE_4.AddYears(-1) : DATA_03_10_INICIO_BIMESTRE_4;

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_FINAL,
                ComponenteCurricular = componente.ToString(),
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula
            };

            await CriarDadosBase(filtroNota);
        }
    }
}
