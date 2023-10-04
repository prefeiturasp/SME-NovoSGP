using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    [CollectionDefinition("TesteIntegradoSGP")]
    public class CollectionDoTeste : ICollectionFixture<CollectionFixture>
    {
    }
}
