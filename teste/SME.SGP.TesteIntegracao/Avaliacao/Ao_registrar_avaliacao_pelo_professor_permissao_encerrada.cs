using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using Xunit;

namespace SME.SGP.TesteIntegracao.AvaliacaoAula
{
    public class Ao_registrar_avaliacao_pelo_professor_permissao_encerrada : TesteAvaliacao
    {
        public Ao_registrar_avaliacao_pelo_professor_permissao_encerrada(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaFalso), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Nao_pode_registrar_avaliacao_professor_com_permissao_encerrada()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_02_05, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await Validar(comando, atividadeAvaliativa);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Inserir(atividadeAvaliativa));

            excecao.Message.ShouldBe(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }

        private static async Task Validar(IComandosAtividadeAvaliativa comando, Infra.AtividadeAvaliativaDto atividadeAvaliativa)
        {
            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa);

            await comando.Validar(filtroAtividadeAvaliativa).ShouldNotThrowAsync();
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto()
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
                Bimestre = BIMESTRE_2
            };
        }
    }


}
