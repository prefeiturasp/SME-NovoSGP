using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_obter_conselho_classe_aluno_query : ConselhoClasseTesteBase
    {
        public Ao_obter_conselho_classe_aluno_query(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        // protected override void RegistrarFakes(IServiceCollection services)
        // {
        //     base.RegistrarFakes(services);
        //
        //     services.Replace(new ServiceDescriptor(
        //         typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
        //         typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake),
        //         ServiceLifetime.Scoped));
        // }

        [Fact]
        public async Task Ao_obter_conselho_classe_aluno_por_conselho_fechamento_aluno_codigo_query()
        {
        }
    }
}