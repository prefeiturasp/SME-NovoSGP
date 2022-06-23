using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
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
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("É necessário informar o componente curricular");
        }

        [Fact]
        public async Task Nao_existe_aula_cadastrada_nesta_data()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
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
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Não foi encontrado nenhum período escolar para essa data.");
        }

        [Fact]
        public async Task Existe_atividade_avaliativa_cadastrada_com_esse_nome_para_o_bimestre()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());
            await CrieAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
            await CriarAtividadeAvaliativaFundamental(DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
            dto.Nome = "Avaliação 04";
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Já existe atividade avaliativa cadastrada com esse nome para esse bimestre.");
        }

        [Fact]
        public async Task Existe_atividade_avaliativa_cadastrada_para_essa_data_e_componente()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());
            await CrieAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
            await CriarAtividadeAvaliativaFundamental(DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Já existe atividade avaliativa cadastrada para essa data e componente curricular.");
        }

        [Fact]
        public async Task Existe_atividade_avaliativa_cadastrada_para_essa_data_e_componente_para_regencia()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(true, false));
            await CrieAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
            await CriarComponenteCurricular();
            await CriarAtividadeAvaliativaFundamental(DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral, true);
            await CriarAtividadeAvaliativaRegencia();

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterFiltro(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05);
            dto.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() };
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Validar(dto));

            excecao.Message.ShouldBe("Já existe atividade avaliativa cadastrada para essa data e componente curricular.");
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto(bool criaPeriodo = true, bool criaComponente = true)
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
                CriarPeriodo = criaPeriodo,
                CriarComponente = criaComponente
            };
        }
    }
}
