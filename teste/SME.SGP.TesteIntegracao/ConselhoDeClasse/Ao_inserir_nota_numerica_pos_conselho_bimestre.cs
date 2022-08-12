using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_nota_numerica_pos_conselho_bimestre : ConselhoDeClasseTesteBase
    {
        public Ao_inserir_nota_numerica_pos_conselho_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_fundamental(bool anoAnterior) 
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CrieDados(
                ObterPerfilProfessor(), 
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
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_fundamental_cp(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CrieDados(ObterPerfilCP(),
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
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_medio(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
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
        public async Task Ao_lancar_nota_pos_conselho_bimestre_numerica_medio_diretor(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
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
        public async Task Ao_lancar_nota_pos_conselho_bimestre_numerica_eja(bool anoAnterior)
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CrieDados(ObterPerfilProfessor(), 
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
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            
            await CrieDados(ObterPerfilProfessor(), 
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
