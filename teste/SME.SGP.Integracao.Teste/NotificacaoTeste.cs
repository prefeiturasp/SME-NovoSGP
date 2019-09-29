using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        [Fact, Order(5)]
        public void Deve_Consultar_Notificacao_Basica_Lista()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.N_C }));

            var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes/resumo?anoLetivo={2019}&usuarioRf={1}").Result;

            Assert.True(getResult.IsSuccessStatusCode);
        }

        [Fact, Order(6)]
        public void Deve_Consultar_Quantidade_Notificacao_Nao_Lidas()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.N_I, Permissao.N_C }));

            var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes/quantidade/naolidas?anoLetivo={2019}&usuarioRf={1}").Result;

            Assert.True(getResult.IsSuccessStatusCode);
        }

        [Fact, Order(2)]
        public void Deve_Incluir_e_Consultar_Alerta()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.N_I, Permissao.N_C }));

            var notificacaoDto = new NotificacaoDto();
            notificacaoDto.Categoria = Dominio.NotificacaoCategoria.Alerta;
            notificacaoDto.Mensagem = "Mensagem de teste 2";
            notificacaoDto.Titulo = "Titulo de Teste 2";
            notificacaoDto.UsuarioRf = "0127254221";
            notificacaoDto.Tipo = Dominio.NotificacaoTipo.Sondagem;

            var jsonParaPost = new StringContent(TransformarEmJson(notificacaoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/notificacoes/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            if (postResult.IsSuccessStatusCode)
            {
                var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioRf={notificacaoDto.UsuarioRf}").Result;
                var notificacoesDto = JsonConvert.DeserializeObject<IEnumerable<NotificacaoBasicaDto>>(getResult.Content.ReadAsStringAsync().Result);

                Assert.True(notificacoesDto.Count() == 1);

                var notificacao = notificacoesDto.FirstOrDefault();

                var getResultDetalhe = _fixture._clientApi.GetAsync($"api/v1/notificacoes/{notificacao.Id}").Result;
                Assert.True(getResultDetalhe.IsSuccessStatusCode);

                var notificacaoDetalheDto = JsonConvert.DeserializeObject<NotificacaoDetalheDto>(getResultDetalhe.Content.ReadAsStringAsync().Result);

                Assert.Equal(notificacaoDetalheDto.Tipo, notificacaoDto.Tipo.GetAttribute<DisplayAttribute>().Name);
                Assert.Equal(notificacaoDetalheDto.Mensagem, notificacaoDto.Mensagem);
                Assert.Equal(notificacaoDetalheDto.Titulo, notificacaoDto.Titulo);
                Assert.Equal(notificacaoDetalheDto.StatusId, (int)NotificacaoStatus.Pendente);
            }
        }

        [Fact, Order(1)]
        public void Deve_Incluir_e_Consultar_Aviso()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.N_I, Permissao.N_C }));

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
                var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioRf={notificacaoDto.UsuarioRf}").Result;
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
                Assert.Equal(notificacaoDetalheDto.StatusId, (int)NotificacaoStatus.Lida);
            }
        }

        [Fact, Order(4)]
        public void Deve_Incluir_e_Excluir()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.N_I, Permissao.N_E, Permissao.N_C }));

            var notificacaoDto = new NotificacaoDto();
            notificacaoDto.Categoria = NotificacaoCategoria.Aviso;
            notificacaoDto.Mensagem = "Notificação de aviso";
            notificacaoDto.Titulo = "Notificação para excluir";
            notificacaoDto.UsuarioRf = "987654333";
            notificacaoDto.Tipo = NotificacaoTipo.Frequencia;

            var jsonParaPost = new StringContent(TransformarEmJson(notificacaoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/notificacoes/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioRf={notificacaoDto.UsuarioRf}").Result;
            var notificacoesDto = JsonConvert.DeserializeObject<IEnumerable<NotificacaoBasicaDto>>(getResult.Content.ReadAsStringAsync().Result);

            var jsonDelete = new StringContent(JsonConvert.SerializeObject(notificacoesDto.Select(c => c.Id)), UnicodeEncoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = jsonDelete,
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{ _fixture._clientApi.BaseAddress}api/v1/notificacoes/")
            };

            var putResult = _fixture._clientApi.SendAsync(request).Result;

            Assert.True(putResult.IsSuccessStatusCode);
            var listaMensagens = JsonConvert.DeserializeObject<IEnumerable<AlteracaoStatusNotificacaoDto>>(putResult.Content.ReadAsStringAsync().Result);
            var id = notificacoesDto.FirstOrDefault().Id;
            Assert.True(listaMensagens.Count(c => c.Sucesso) == 1);
        }

        [Fact, Order(3)]
        public void Deve_Marcar_Como_Lida()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.N_I, Permissao.N_A, Permissao.N_C }));

            var notificacaoDto = new NotificacaoDto();
            notificacaoDto.Categoria = NotificacaoCategoria.Alerta;
            notificacaoDto.Mensagem = "Notificação de alerta";
            notificacaoDto.Titulo = "Titulo de Teste de alerta";
            notificacaoDto.UsuarioRf = "987654321";
            notificacaoDto.Tipo = NotificacaoTipo.Frequencia;

            var jsonParaPost = new StringContent(TransformarEmJson(notificacaoDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/notificacoes/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            notificacaoDto.Categoria = NotificacaoCategoria.Aviso;

            jsonParaPost = new StringContent(TransformarEmJson(notificacaoDto), UnicodeEncoding.UTF8, "application/json");

            postResult = _fixture._clientApi.PostAsync("api/v1/notificacoes/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioRf={notificacaoDto.UsuarioRf}").Result;
            var notificacoesDto = JsonConvert.DeserializeObject<IEnumerable<NotificacaoBasicaDto>>(getResult.Content.ReadAsStringAsync().Result);

            var jsonPut = new StringContent(JsonConvert.SerializeObject(notificacoesDto.Select(c => c.Id)), UnicodeEncoding.UTF8, "application/json");
            var putResult = _fixture._clientApi.PutAsync($"api/v1/notificacoes/status/lida", jsonPut).Result;

            Assert.True(putResult.IsSuccessStatusCode);
            getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?UsuarioRf={notificacaoDto.UsuarioRf}").Result;
            notificacoesDto = JsonConvert.DeserializeObject<IEnumerable<NotificacaoBasicaDto>>(getResult.Content.ReadAsStringAsync().Result);
            Assert.Contains(notificacoesDto, c => c.Status == NotificacaoStatus.Lida && c.Categoria == NotificacaoCategoria.Alerta);
            Assert.Contains(notificacoesDto, c => c.Status == NotificacaoStatus.Pendente && c.Categoria == NotificacaoCategoria.Aviso);
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}