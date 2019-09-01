using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [CollectionDefinition("Testserver collection")]
    public class TestserverCollection : ICollectionFixture<TestServerFixture>
    {
    }
}