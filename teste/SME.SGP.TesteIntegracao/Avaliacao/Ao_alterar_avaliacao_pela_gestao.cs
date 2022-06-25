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

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_alterar_avaliacao_pela_gestao : TesteAvaliacao
    {
        private readonly DateTime DATA_24_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 24);

        private const long TIPO_CALENDARIO_1 = 1;
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
            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TIPO_CALENDARIO_1);
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(
            COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
            CategoriaAtividadeAvaliativa.Normal,
            DATA_02_05,
            TipoAvaliacaoCodigo.AvaliacaoBimestral);
            atividadeAvaliativa.DisciplinaContidaRegenciaId = new string[] { COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString() };

            await comando.Inserir(atividadeAvaliativa);

            atividadeAvaliativa.Descricao = DESCRICAO;

            var retorno = await comando.Alterar(atividadeAvaliativa, 1);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();

            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
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

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Inserir(atividadeAvaliativa));
            excecao.Message.ShouldBe("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
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
