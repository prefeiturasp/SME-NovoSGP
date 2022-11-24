using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AvaliacaoAula
{
    public class Ao_alterar_avaliacao : TesteAvaliacao
    {
        public Ao_alterar_avaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Alterar_avaliacao_para_professor_especialista()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(ObterPerfilProfessor()));

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TipoAvaliacaoCodigo.AvaliacaoMensal);

            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await CriarAula(DATA_02_05, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            dto.DisciplinasId = new string[] { COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() };

            await CriarPeriodoEscolarEAbertura();

            await ExecuteTesteAlterarAvaliacaoPorPerfil(dto);

            var atividadeAvaliativasDiciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeAvaliativasDiciplina.ShouldNotBeEmpty();

            atividadeAvaliativasDiciplina.Exists(disciplina => disciplina.DisciplinaId == COMPONENTE_GEOGRAFIA_ID_8).ShouldBe(true);
        }

        [Fact]
        public async Task Alterar_avaliacao_para_professor_regente_de_classe()
        {
            await Executa_alterar_avaliacao_para_professor_regente_de_classe();
        }

        [Fact]
        public async Task Alterar_avaliacao_para_professor_regente_de_classe_com_rf_diferentes()
        {
            await Executa_alterar_avaliacao_para_professor_regente_de_classe(USUARIO_PROFESSOR_CODIGO_RF_1111111);
        }

        [Fact]
        public async Task Nao_pode_alterar_esta_atividade_cj_para_rf_diferentes()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(ObterPerfilCJ()));

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TipoAvaliacaoCodigo.AvaliacaoMensal, true, true, USUARIO_PROFESSOR_CODIGO_RF_1111111);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await CriarAula(DATA_02_05, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TIPO_CALENDARIO_1,true);

            await CriarPeriodoEscolarEAbertura();

            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa);

            await comando.Validar(filtroAtividadeAvaliativa).ShouldNotThrowAsync();

            await comando.Alterar(atividadeAvaliativa, 1).ShouldThrowAsync<NegocioException>();
        }

        private async Task Executa_alterar_avaliacao_para_professor_regente_de_classe(string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(ObterPerfilProfessor()));

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TipoAvaliacaoCodigo.AvaliacaoMensal);

            await CriarAtividadeAvaliativaRegencia(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105);

            await CriarAula(DATA_02_05, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TIPO_CALENDARIO_1);

            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            dto.DisciplinasId = new string[] { COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            dto.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await CriarPeriodoEscolarEAbertura();

            await ExecuteTesteAlterarAvaliacaoPorPerfil(dto);

            var atividadeAvaliativasDiciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeAvaliativasDiciplina.ShouldNotBeEmpty();

            atividadeAvaliativasDiciplina.Exists(disciplina => disciplina.DisciplinaId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString()).ShouldBe(true);

            var atividadeRegencia = ObterTodos<AtividadeAvaliativaRegencia>();

            atividadeRegencia.ShouldNotBeEmpty();

            atividadeRegencia.Exists(disciplina => disciplina.DisciplinaContidaRegenciaId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString()).ShouldBe(true);
        }

        private async Task ExecuteTesteAlterarAvaliacaoPorPerfil(AtividadeAvaliativaDto atividadeAvaliativa)
        {
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await CriarPeriodoEscolarEAbertura();

            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa,1);

            async Task doExecutar() { await comando.Validar(filtroAtividadeAvaliativa); }

            await Should.NotThrowAsync(() => doExecutar());

            var retorno = await comando.Alterar(atividadeAvaliativa, 1);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();

            atividadeAvaliativas.FirstOrDefault().TipoAvaliacaoId.ShouldBe((long)TipoAvaliacaoCodigo.AvaliacaoBimestral);
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
