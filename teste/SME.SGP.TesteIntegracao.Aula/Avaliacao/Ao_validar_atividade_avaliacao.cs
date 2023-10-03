using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AvaliacaoAula
{
    public class Ao_validar_atividade_avaliacao : TesteAvaliacao
    {
        public Ao_validar_atividade_avaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task E_necessario_informar_o_componente_curricular()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var dto = ObterFiltro(string.Empty, DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("É necessário informar o componente curricular");
        }

        [Fact]
        public async Task Nao_existe_aula_cadastrada_nesta_data()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Não existe aula cadastrada nesta data.");
        }

        [Fact]
        public async Task Nao_foi_encontrado_nenhum_perido_escolar_para_data()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(false));

            await CrieAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_ID);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Não foi encontrado nenhum período escolar para essa data.");
        }

        [Fact]
        public async Task Existe_atividade_avaliativa_cadastrada_com_esse_nome_para_o_bimestre()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CrieAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);

            dto.Nome = "Avaliação 04";

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Já existe atividade avaliativa cadastrada com esse nome para esse bimestre.");
        }

        [Fact]
        public async Task Existe_atividade_avaliativa_cadastrada_para_essa_data_e_componente()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CrieAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Já existe atividade avaliativa cadastrada para essa data e componente curricular.");
        }

        [Fact]
        public async Task Existe_atividade_avaliativa_cadastrada_para_essa_data_e_componente_para_regencia()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CrieAula(COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), DATA_02_05);

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), TipoAvaliacaoCodigo.AvaliacaoBimestral, true);

            await CriarAtividadeAvaliativaRegencia(COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var dto = ObterFiltro(COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), DATA_02_05);

            dto.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Já existe atividade avaliativa cadastrada para essa data e componente curricular.");
        }

        [Fact]
        public async Task Existe_atividade_avaliativa_cadastrada_para_essa_data_e_componente_para_regencia_bimestre_passado()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CrieAula(COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), DATA_10_01);

            await CriarAtividadeAvaliativaFundamental(DATA_10_01, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), TipoAvaliacaoCodigo.AvaliacaoBimestral, true);

            await CriarAtividadeAvaliativaRegencia(COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var dto = ObterFiltro(COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), DATA_10_01);

            dto.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Já existe atividade avaliativa cadastrada para essa data e componente curricular.");
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_28_04, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_ID);
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto(bool criaPeriodo = true)
        {
            return new CriacaoDeDadosDto()
            {
                Perfil = ObterPerfilProfessor(),
                ModalidadeTurma = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoCalendarioId = TIPO_CALENDARIO_ID,
                DataInicio = DATA_02_05,
                DataFim = DATA_08_07,
                TipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Bimestre = BIMESTRE_2,
                CriarPeriodo = criaPeriodo
            };
        }
    }
}
