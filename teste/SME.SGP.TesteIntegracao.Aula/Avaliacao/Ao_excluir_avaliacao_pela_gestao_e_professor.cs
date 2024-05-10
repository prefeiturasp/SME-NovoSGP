using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AvaliacaoAula
{
    public class Ao_excluir_avaliacao_pela_gestao_e_professor : TesteAvaliacao
    {

        public Ao_excluir_avaliacao_pela_gestao_e_professor(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Excluir_avaliacao_pelo_cp()
        {
            await ExecuteExclusao(ObterPerfilCP(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
        }

        [Fact]
        public async Task Excluir_avaliacao_pelo_diretor()
        {
            await ExecuteExclusao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
        }

        [Fact]
        public async Task Excluir_avaliacao_pelo_professor()
        {
            await ExecuteExclusao(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
        }

        [Fact]
        public async Task Deve_permitir_excluir_avaliacao_pelo_professor_bimestre_passado()
        {            
            await ExecuteExclusaoComData(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_10_01);
        }

        [Fact]
        public async Task Excluir_avaliacao_pelo_professor_regente_diferente()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(ObterPerfilProfessor()));

            await CrieAula(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), DATA_02_05);

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TipoAvaliacaoCodigo.AvaliacaoMensal, true, false, USUARIO_PROFESSOR_CODIGO_RF_1111111);

            await CriarAtividadeAvaliativaRegencia(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await CriarPeriodoEscolarEAbertura();

            await comando.Excluir(1);

            ExecuteValidacao();

            var atividadeRegencia = ObterTodos<AtividadeAvaliativaRegencia>();

            atividadeRegencia.ShouldNotBeEmpty();

            atividadeRegencia.FirstOrDefault().Excluido.ShouldBe(true);
        }

        [Fact]
        public async Task Nao_foi_possivel_localizar_avaliacao()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(ObterPerfilDiretor()));
            
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Excluir(1));

            excecao.Message.ShouldBe("Não foi possível localizar esta avaliação.");
        }

        private async Task ExecuteExclusao(string perfil, string componente)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil));

            await CrieAula(componente, DATA_02_05);

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, componente, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await CriarPeriodoEscolarEAbertura();

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await comando.Excluir(1);

            ExecuteValidacao();
        }

        private async Task ExecuteExclusaoComData(string perfil, string componente, DateTime dataAula)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil));

            await CrieAula(componente, dataAula);

            await CriarAtividadeAvaliativaFundamental(dataAula, componente, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await CriarPeriodoEscolarEAbertura();

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await comando.Excluir(1);

            ExecuteValidacao();
        }

        private void ExecuteValidacao()
        {
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

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_28_04, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_ID);
        }
    }
}
