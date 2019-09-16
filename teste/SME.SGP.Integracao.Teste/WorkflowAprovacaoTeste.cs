using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class WorkflowAprovacaoTeste
    {
        private readonly TestServerFixture fixture;

        public WorkflowAprovacaoTeste(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Deve_Inserir_e_Consultar_LinhaTempo()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();

            //var wfAprovacao = new WorkflowAprovacaoDto
            //{
            //     NotificacaoCategoria = Dominio.NotificacaoCategoria.Workflow_Aprovacao,
            //      NotificacaoMensagem = "Mensagem de teste"
            //};

            //var post = JsonConvert.SerializeObject(wfAprovacao);

            //var jsonParaPost = new StringContent(post, UnicodeEncoding.UTF8, "application/json");

            //var postResult = _fixture._clientApi.PostAsync("api/v1/supervisores/atribuir-ue", jsonParaPost).Result;

            //var getResult = fixture._clientApi.GetAsync("api/v1/professores/6082840/turmas/1982186/disciplinas/").Result;
        }
    }
}