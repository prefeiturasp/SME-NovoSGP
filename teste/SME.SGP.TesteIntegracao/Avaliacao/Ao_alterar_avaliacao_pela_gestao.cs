using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AvaliacaoAula
{
    public class Ao_alterar_avaliacao_pela_gestao : TesteAvaliacao
    {
        private const string DESCRICAO = "OUTRA DESCRICAO";

        public Ao_alterar_avaliacao_pela_gestao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaFalso), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Alterar_avaliacao_pelo_gestor_Diretor()
        {
            await ExecutarTesteParaGestor(ObterPerfilDiretor());
        }

        [Fact]
        public async Task Alterar_avaliacao_pelo_gestor_CP()
        {
            await ExecutarTesteParaGestor(ObterPerfilCP());
        }

        [Fact]
        public async Task Alterar_avaliacao_pelo_gestor_AD()
        {
            await ExecutarTesteParaGestor(ObterPerfilAD());
        }

        [Fact]
        public async Task Nao_deve_alterar_avaliacao_se_nao_for_gestor()
        {
            await ExecutarTesteParaNaoGestor(ObterPerfilProfessor());
        }

        private async Task ExecutarTesteParaGestor(string perfil)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil));

            await CriarAula(DATA_02_05, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), TIPO_CALENDARIO_1);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(
            COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
            CategoriaAtividadeAvaliativa.Normal,
            DATA_02_05,
            TipoAvaliacaoCodigo.AvaliacaoBimestral);

            atividadeAvaliativa.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await CriarPeriodoEscolarEAbertura();

            await Validar(comando, atividadeAvaliativa);

            await comando.Inserir(atividadeAvaliativa);

            atividadeAvaliativa.Descricao = DESCRICAO;

            await Validar(comando, atividadeAvaliativa,1);

            var retorno = await comando.Alterar(atividadeAvaliativa, 1);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();

            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        private static async Task Validar(IComandosAtividadeAvaliativa comando, Infra.AtividadeAvaliativaDto atividadeAvaliativa, int avaliacaoId = 0)
        {
            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa, avaliacaoId);

            await comando.Validar(filtroAtividadeAvaliativa).ShouldNotThrowAsync();
        }

        private async Task ExecutarTesteParaNaoGestor(string perfil)
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto(perfil));
            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TIPO_CALENDARIO_1);
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(
            COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
            CategoriaAtividadeAvaliativa.Normal,
            DATA_02_05,
            TipoAvaliacaoCodigo.AvaliacaoBimestral);
            atividadeAvaliativa.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await comando.Inserir(atividadeAvaliativa).ShouldThrowAsync<NegocioException>();
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
