using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_excluir_avaliacao_pela_gestao : TesteAvaliacao
    {
        public Ao_excluir_avaliacao_pela_gestao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Excluir_avaliacao_pelo_cp()
        {
            await ExecuteExclusao(ObterPerfilCP());
        }

        [Fact]
        public async Task Excluir_avaliacao_pelo_diretor()
        {
            await ExecuteExclusao(ObterPerfilDiretor());
        }

        [Fact]
        public async Task Nao_foi_possivel_localizar_avaliacao()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(ObterPerfilDiretor()));
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Excluir(1));

            excecao.Message.ShouldBe("Não foi possível localizar esta avaliação.");
        }

        private async Task ExecuteExclusao(string perfil)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil));
            await CrieAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await comando.Excluir(1);

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.FirstOrDefault().Excluido.ShouldBe(true);

            var atividadeDisciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeDisciplina.ShouldNotBeEmpty();
            atividadeDisciplina.FirstOrDefault().Excluido.ShouldBe(true);
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
