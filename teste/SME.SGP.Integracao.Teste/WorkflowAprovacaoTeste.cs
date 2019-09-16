using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class WorkflowAprovacaoTeste
    {
        private readonly TestServerFixture fixture;

        public WorkflowAprovacaoTeste(TestServerFixture fixture)
        {
            fixture = fixture;
        }

        public void Deve_Inserir_e_Consultar_LinhaTempo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            var getResult = fixture._clientApi.GetAsync("api/v1/professores/6082840/turmas/1982186/disciplinas/").Result;
        }
    }
}