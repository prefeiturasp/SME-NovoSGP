using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_registrar_avaliacao_pela_gestao : TesteAvaliacao
    {
        public Ao_registrar_avaliacao_pela_gestao(CollectionFixture collectionFixture) : base(collectionFixture)
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

        private async Task ExecuteTesteResgistrarAvaliacaoPorRegencia(string perfil)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil, false));
            await CriarComponenteCurricular();

            var dto = ObterAtividadeAvaliativaDto(
                        COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), 
                        CategoriaAtividadeAvaliativa.Normal, 
                        DATA_02_05, 
                        TipoAvaliacaoCodigo.AvaliacaoBimestral);
            dto.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await ExecuteTesteResgistrarAvaliacaoPorPerfil(perfil, dto);

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativaRegencia>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        private async Task ExecuteTesteResgistrarAvaliacao(string perfil)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil, true));

            var dto = ObterAtividadeAvaliativaDto(
                            COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), 
                            CategoriaAtividadeAvaliativa.Normal, 
                            DATA_02_05, 
                            TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await ExecuteTesteResgistrarAvaliacaoPorPerfil(perfil, dto);
        }

        private async Task ExecuteTesteResgistrarAvaliacaoPorPerfil(string perfil, AtividadeAvaliativaDto dto)
        {
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var retorno = await comando.Inserir(dto);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto(string perfil, bool criaComponente)
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
                Bimestre = BIMESTRE_2,
                CriarComponente = criaComponente
            };
        }
    }
}
