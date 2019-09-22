using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class AutenticacaoTeste
    {
        private readonly TestServerFixture fixture;

        public AutenticacaoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        public void Deve_Retornar_Nao_Autorizado()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("login", "4/P7q7W91a-oMsCeLvIaQm6bTrgtp7") },
                { new KeyValuePair<string, string>("senha", "ABCD1234") }
                                                                                                    };

            var postResult = fixture._clientApi.PostAsync($"api/v1/autenticacao", new FormUrlEncodedContent(nameValueCollection)).Result;
            Assert.True(!postResult.IsSuccessStatusCode);
            Assert.True(postResult.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }
    }
}