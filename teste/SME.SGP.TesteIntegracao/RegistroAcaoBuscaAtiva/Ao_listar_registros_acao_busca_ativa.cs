using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_listar_registros_acao_busca_ativa : RegistroAcaoBuscaAtivaTesteBase
    {
        
   
        public Ao_listar_registros_acao_busca_ativa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            /*services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigoPorIdQuery, string>), typeof(ObterTurmaCodigoPorIdQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));*/
        }

        [Fact(DisplayName = "Ao listar as seções de registro de ação - sem Id registro")]
        public async Task Ao_listar_secoes_registro_acao()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);
            var useCase = ObterUseCaseListagemSecoes();
            var retorno = await useCase.Executar(new FiltroSecoesDeRegistroAcao());
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(1);
            retorno.All(r => !r.Concluido).ShouldBeTrue();
            retorno.All(r => r.Auditoria.EhNulo()).ShouldBeTrue();
        }
    }
}

