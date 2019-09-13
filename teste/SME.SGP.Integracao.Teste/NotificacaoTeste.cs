using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            notificacaoDto.Titulo = "Titulo de Teste";
            notificacaoDto.UsuarioRf = "987654321";

            notificacaoDto.Tipo = Dominio.NotificacaoTipo.Frequencia;

            var jsonParaPost = new StringContent(TransformarEmJson(notificacaoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/notificacoes/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            if (postResult.IsSuccessStatusCode)
            {
                var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioId={notificacaoDto.UsuarioRf}").Result;
                var notificacoesDto = JsonConvert.DeserializeObject<IEnumerable<NotificacaoBasicaDto>>(getResult.Content.ReadAsStringAsync().Result);

                Assert.True(notificacoesDto.Count() == 1);

                var getResult2 = _fixture._clientApi.GetAsync($"api/v1/notificacoes?Tipo={(int)Dominio.NotificacaoTipo.Notas}").Result;
                var notificacoesDto2 = JsonConvert.DeserializeObject<IEnumerable<NotificacaoBasicaDto>>(getResult2.Content.ReadAsStringAsync().Result);
                Assert.True(notificacoesDto2.Count() == 0);

                var getResultDetalhe = _fixture._clientApi.GetAsync($"api/v1/notificacoes/{notificacoesDto.FirstOrDefault().Id}").Result;

                Assert.True(getResultDetalhe.IsSuccessStatusCode);

                var notificacaoDetalheDto = JsonConvert.DeserializeObject<NotificacaoDetalheDto>(getResultDetalhe.Content.ReadAsStringAsync().Result);

                Assert.Equal(notificacaoDto.Tipo.GetAttribute<DisplayAttribute>().Name, notificacaoDetalheDto.Tipo);
                Assert.Equal(notificacaoDto.Mensagem, notificacaoDetalheDto.Mensagem);
                Assert.Equal(notificacaoDto.Titulo, notificacaoDetalheDto.Titulo);
            }
        }

        [Fact, Order(2)]
        public void DevemarcarComoLida()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            var notificacaoDto = new NotificacaoDto();
            notificacaoDto.Categoria = Dominio.NotificacaoCategoria.Alerta;
            notificacaoDto.Mensagem = "Notificação de alerta";
            notificacaoDto.Titulo = "Titulo de Teste de alerta";
            notificacaoDto.UsuarioRf = "987654321";
            notificacaoDto.Tipo = Dominio.NotificacaoTipo.Frequencia;

            var jsonParaPost = new StringContent(TransformarEmJson(notificacaoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/notificacoes/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioId={notificacaoDto.UsuarioRf}").Result;
            var notificacoesDto = JsonConvert.DeserializeObject<IEnumerable<NotificacaoBasicaDto>>(getResult.Content.ReadAsStringAsync().Result);

            var putResult = _fixture._clientApi.PutAsync($"api/v1/notificacoes/{notificacoesDto.FirstOrDefault(c => c.Titulo == "Titulo de Teste de alerta").Id}/marcar-como-lida", null).Result;
            Assert.True(putResult.IsSuccessStatusCode);
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}