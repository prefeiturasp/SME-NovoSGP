using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class EmailTeste
    {
        private readonly TestServerFixture _fixture;

        public EmailTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(1)]
        public void Email()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            var postResult = _fixture._clientApi.PostAsync($"api/v1/autenticacao/testeemail", null).Result;

            Assert.True(postResult.IsSuccessStatusCode);
        }
    }
}