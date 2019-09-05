using Newtonsoft.Json;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class NotificacaoTeste
    {
        private readonly TestServerFixture _fixture;

        public NotificacaoTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(1)]
        public void Deve_Incluir_e_Consultar()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            var notificacaoDto = new NotificacaoDto();
            notificacaoDto.Categoria = Dominio.NotificacaoCategoria.Aviso;
            notificacaoDto.Mensagem = "Mensagem de teste";
            notificacaoDto.PodeRemover = true;
            notificacaoDto.Titulo = "Titulo de Teste";
            notificacaoDto.UsuarioId = "1230";
            notificacaoDto.Tipo = Dominio.NotificacaoTipo.Frequencia;

            var jsonParaPost = new StringContent(TransformarEmJson(notificacaoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/notificacoes/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            if (postResult.IsSuccessStatusCode)
            {
                var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioId={notificacaoDto.UsuarioId}").Result;
                var notificacoesDto = JsonConvert.DeserializeObject<IEnumerable<PlanoCicloCompletoDto>>(getResult.Content.ReadAsStringAsync().Result);

                Assert.True(notificacoesDto.Count() == 1);

                var getResult2 = _fixture._clientApi.GetAsync($"api/v1/notificacoes?Tipo={(int)Dominio.NotificacaoTipo.Notas}").Result;
                var notificacoesDto2 = JsonConvert.DeserializeObject<IEnumerable<PlanoCicloCompletoDto>>(getResult2.Content.ReadAsStringAsync().Result);
                Assert.True(notificacoesDto2.Count() == 0);
            }
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}