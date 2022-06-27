using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
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

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_02_05, TipoAvaliacaoCodigo.AvaliacaoBimestral);
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Inserir(dto));

            excecao.Message.ShouldBe("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
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
