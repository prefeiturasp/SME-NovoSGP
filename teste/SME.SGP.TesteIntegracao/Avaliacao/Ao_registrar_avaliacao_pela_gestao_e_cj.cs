using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_registrar_avaliacao_pela_gestao_e_cj : TesteAvaliacao
    {
        public Ao_registrar_avaliacao_pela_gestao_e_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Registrar_avaliacao_para_gestor_cp()
        {
            await ExecuteTesteResgistrarAvaliacao(ObterPerfilCP());
        }

        [Fact]
        public async Task Registrar_avaliacao_para_gestor_diretor()
        {
            await ExecuteTesteResgistrarAvaliacao(ObterPerfilDiretor());
        }

        [Fact]
        public async Task Registrar_avaliacao_para_gestor_cp_regente_de_classe()
        {
            await ExecuteTesteResgistrarAvaliacaoPorRegencia(ObterPerfilCP());
        }

        [Fact]
        public async Task Registrar_avaliacao_para_gestor_diretor_regente_de_classe()
        {
            await ExecuteTesteResgistrarAvaliacaoPorRegencia(ObterPerfilDiretor());
        }

        [Fact]
        public async Task Registrar_avaliacao_professor_cj()
        {
            await ExecuteTesteResgistrarAvaliacao(ObterPerfilCJ());
        }

        [Fact]
        public async Task Registrar_avaliacao_professor_cj_regente()
        {
            await ExecuteTesteResgistrarAvaliacaoPorRegencia(ObterPerfilCJ());
        }

        private async Task ExecuteTesteResgistrarAvaliacaoPorRegencia(string perfil)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil));

            var dto = ObterAtividadeAvaliativaDto(
                        COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), 
                        CategoriaAtividadeAvaliativa.Normal, 
                        DATA_02_05, 
                        TipoAvaliacaoCodigo.AvaliacaoBimestral);
            dto.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await ExecuteTesteResgistrarAvaliacaoPorPerfilRegente(dto);
        }

        private async Task ExecuteTesteResgistrarAvaliacao(string perfil)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil));

            var dto = ObterAtividadeAvaliativaDto(
                            COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), 
                            CategoriaAtividadeAvaliativa.Normal, 
                            DATA_02_05, 
                            TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await ExecuteTesteResgistrarAvaliacaoPorPerfil(dto);
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto(string perfil)
        {
            return new CriacaoDeDadosDto()
            {
                Perfil = perfil,
                ModalidadeTurma = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoCalendarioId = TIPO_CALENDARIO_ID,
                DataInicio = DATA_02_05,
                DataFim = DATA_08_07,
                TipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Bimestre = BIMESTRE_2
            };
        }
    }
}
