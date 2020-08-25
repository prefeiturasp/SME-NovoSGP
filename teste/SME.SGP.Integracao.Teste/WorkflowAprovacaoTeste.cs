using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class WorkflowAprovacaoTeste
    {
        private readonly TestServerFixture _fixture;

        public WorkflowAprovacaoTeste(TestServerFixture fixture)
        {
            this._fixture = fixture;
        }

        //TODO: CHAVE INTEGRAÇÃO API EOL
        //[Fact]
        //public void Deve_Inserir_Consultar_LinhaTempo_Aprovar_E_Reprovar()
        //{
        //    _fixture._clientApi.DefaultRequestHeaders.Clear();

        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.N_C }));

        //    var tituloParaLocalizarRegistro = Guid.NewGuid().ToString();

        //    var wfAprovacao = new WorkflowAprovacaoDto
        //    {
        //        NotificacaoCategoria = Dominio.NotificacaoCategoria.Workflow_Aprovacao,
        //        NotificacaoMensagem = "Mensagem de teste",
        //        NotificacaoTipo = Dominio.NotificacaoTipo.Fechamento,
        //        NotificacaoTitulo = tituloParaLocalizarRegistro,
        //        Tipo = WorkflowAprovacaoTipo.Basica,
        //        UeId = "000892"
        //    };

        //    wfAprovacao.Niveis.Add(new WorkflowAprovacaoNivelDto()
        //    {
        //        Cargo = Dominio.Cargo.Diretor,
        //        Nivel = 1
        //    });
        //    wfAprovacao.Niveis.Add(new WorkflowAprovacaoNivelDto()
        //    {
        //        Cargo = Dominio.Cargo.Diretor,
        //        Nivel = 2
        //    });
        //    wfAprovacao.Niveis.Add(new WorkflowAprovacaoNivelDto()
        //    {
        //        Cargo = Dominio.Cargo.Diretor,
        //        Nivel = 3
        //    });

        //    var post = JsonConvert.SerializeObject(wfAprovacao);

        //    var jsonParaPost = new StringContent(post, UnicodeEncoding.UTF8, "application/json");

        //    var postResult = _fixture._clientApi.PostAsync("api/v1/workflows/aprovacoes", jsonParaPost).Result;

        //    Assert.True(postResult.IsSuccessStatusCode);

        //    if (postResult.IsSuccessStatusCode)
        //    {
        //        var getResult = _fixture._clientApi.GetAsync($"api/v1/notificacoes?titulo={tituloParaLocalizarRegistro}").Result;

        //        Assert.True(getResult.IsSuccessStatusCode);

        //        if (getResult.IsSuccessStatusCode)
        //        {
        //            var notificacoes = JsonConvert.DeserializeObject<PaginacaoResultadoDto<NotificacaoBasicaDto>>(getResult.Content.ReadAsStringAsync().Result);
        //            Assert.True(notificacoes.Items.Count() > 0);
        //            if (notificacoes.Items.Count() > 0)
        //            {
        //                var notificacao = notificacoes.Items.FirstOrDefault();

        //                var getResultTimeline = _fixture._clientApi.GetAsync($"api/v1/workflows/aprovacoes/notificacoes/{notificacao.Id}/linha-tempo").Result;
        //                Assert.True(getResultTimeline.IsSuccessStatusCode);
        //                if (getResultTimeline.IsSuccessStatusCode)
        //                {
        //                    var timeline = JsonConvert.DeserializeObject<List<WorkflowAprovacaoTimeRespostaDto>>(getResultTimeline.Content.ReadAsStringAsync().Result);
        //                    if (timeline != null)
        //                    {
        //                        Assert.True(timeline.Count() == 3);
        //                        Assert.True(timeline.FirstOrDefault(a => a.Nivel == 1).StatusId == (int)WorkflowAprovacaoNivelStatus.AguardandoAprovacao);
        //                        Assert.True(timeline.FirstOrDefault(a => a.Nivel == 2).StatusId == (int)WorkflowAprovacaoNivelStatus.SemStatus);
        //                        Assert.True(timeline.FirstOrDefault(a => a.Nivel == 3).StatusId == (int)WorkflowAprovacaoNivelStatus.SemStatus);

        //                        //Aprovar 1 nível;
        //                        var aprovacaoNivel = new WorkflowAprovacaoAprovacaoDto();
        //                        aprovacaoNivel.Aprova = true;

        //                        var postAprovacaoNivel = JsonConvert.SerializeObject(aprovacaoNivel);

        //                        var jsonParaPostAprovacaoNivel = new StringContent(postAprovacaoNivel, UnicodeEncoding.UTF8, "application/json");

        //                        var postResultAprovacaoNivel = _fixture._clientApi.PutAsync($"api/v1/workflows/aprovacoes/notificacoes/{notificacao.Id}/aprova", jsonParaPostAprovacaoNivel).Result;

        //                        Assert.True(postResultAprovacaoNivel.IsSuccessStatusCode);

        //                        if (postResultAprovacaoNivel.IsSuccessStatusCode)
        //                        {
        //                            var getResultMensagemNivel2 = _fixture._clientApi.GetAsync($"api/v1/notificacoes?titulo={tituloParaLocalizarRegistro}&status=1").Result;

        //                            Assert.True(getResultMensagemNivel2.IsSuccessStatusCode);

        //                            if (getResultMensagemNivel2.IsSuccessStatusCode)
        //                            {
        //                                var notificacoesNivel2 = JsonConvert.DeserializeObject<PaginacaoResultadoDto<NotificacaoBasicaDto>>(getResultMensagemNivel2.Content.ReadAsStringAsync().Result);
        //                                Assert.True(notificacoesNivel2.Items.Count() == 1);
        //                                if (notificacoesNivel2.Items.Count() == 1)
        //                                {
        //                                    var notificacaoNivel2 = notificacoesNivel2.Items.FirstOrDefault();

        //                                    var getResultTimelineNivel2 = _fixture._clientApi.GetAsync($"api/v1/workflows/aprovacoes/notificacoes/{notificacaoNivel2.Id}/linha-tempo").Result;
        //                                    Assert.True(getResultTimeline.IsSuccessStatusCode);
        //                                    if (getResultTimelineNivel2.IsSuccessStatusCode)
        //                                    {
        //                                        var timelineNivel2 = JsonConvert.DeserializeObject<List<WorkflowAprovacaoTimeRespostaDto>>(getResultTimelineNivel2.Content.ReadAsStringAsync().Result);
        //                                        if (timelineNivel2 != null)
        //                                        {
        //                                            Assert.True(timelineNivel2.Count() == 3);
        //                                            Assert.True(timelineNivel2.FirstOrDefault(a => a.Nivel == 1).StatusId == (int)WorkflowAprovacaoNivelStatus.Aprovado);
        //                                            Assert.True(timelineNivel2.FirstOrDefault(a => a.Nivel == 2).StatusId == (int)WorkflowAprovacaoNivelStatus.AguardandoAprovacao);
        //                                            Assert.True(timelineNivel2.FirstOrDefault(a => a.Nivel == 3).StatusId == (int)WorkflowAprovacaoNivelStatus.SemStatus);
        //                                            Assert.True(notificacoes.Items.FirstOrDefault().Codigo == notificacoesNivel2.Items.FirstOrDefault().Codigo);

        //                                            //Reprovacao 2 nivel
        //                                            var reprovacaoNivel = new WorkflowAprovacaoAprovacaoDto();
        //                                            reprovacaoNivel.Aprova = false;
        //                                            reprovacaoNivel.Observacao = "Observação de teste!";

        //                                            var postReprovacaoNivel = JsonConvert.SerializeObject(reprovacaoNivel);

        //                                            var jsonParaPutReprovacaoNivel = new StringContent(postReprovacaoNivel, UnicodeEncoding.UTF8, "application/json");

        //                                            var putResultReprovacaoNivel = _fixture._clientApi.PutAsync($"api/v1/workflows/aprovacoes/notificacoes/{notificacaoNivel2.Id}/aprova", jsonParaPutReprovacaoNivel).Result;

        //                                            Assert.True(putResultReprovacaoNivel.IsSuccessStatusCode);
        //                                            if (putResultReprovacaoNivel.IsSuccessStatusCode)
        //                                            {
        //                                                getResultTimelineNivel2 = _fixture._clientApi.GetAsync($"api/v1/workflows/aprovacoes/notificacoes/{notificacaoNivel2.Id}/linha-tempo").Result;
        //                                                timelineNivel2 = JsonConvert.DeserializeObject<List<WorkflowAprovacaoTimeRespostaDto>>(getResultTimelineNivel2.Content.ReadAsStringAsync().Result);
        //                                                Assert.True(timelineNivel2.Count() == 3);
        //                                                Assert.True(timelineNivel2.FirstOrDefault(a => a.Nivel == 1).StatusId == (int)WorkflowAprovacaoNivelStatus.Aprovado);
        //                                                Assert.True(timelineNivel2.FirstOrDefault(a => a.Nivel == 2).StatusId == (int)WorkflowAprovacaoNivelStatus.Reprovado);
        //                                                Assert.True(timelineNivel2.FirstOrDefault(a => a.Nivel == 3).StatusId == (int)WorkflowAprovacaoNivelStatus.SemStatus);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}