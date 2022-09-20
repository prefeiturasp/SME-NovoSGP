using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE
{
    public class Ao_listar_com_filtros: EncaminhamentoAEETesteBase
    {
        public Ao_listar_com_filtros(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        // protected override void RegistrarFakes(IServiceCollection services)
        // {
        //     base.RegistrarFakes(services);
        //
        //     services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        //     services.Replace(new ServiceDescriptor(typeof(IServicoAuditoria),typeof(ServicoAuditoriaFake), ServiceLifetime.Scoped));
        //     
        // }

        [Fact]
        public async Task Altera_quantidade_de_aulas_com_recorrencia_no_bimestre_atual()
        {
            var obterEncaminhamentosAeeUseCase = ObterServicoListagemComFiltros();
            
            
            
            
        }
    }
}