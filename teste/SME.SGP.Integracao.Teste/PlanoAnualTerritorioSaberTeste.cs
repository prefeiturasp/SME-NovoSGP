using SME.SGP.Infra.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class PlanoAnualTerritorioSaberTeste
    {
        private readonly TestServerFixture _fixture;

        public PlanoAnualTerritorioSaberTeste(TestServerFixture fixture)
        {
            this._fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }


        /// COMENTADO POIS PRECISA DO USUÁRIO LOGADO PRA FUNCIONAR
        
        //[Fact, Order(3)]
        //public void DeveIncluirPlanoAnualTerritorioSaber()
        //{
        //    try
        //    {
        //        _fixture._clientApi.DefaultRequestHeaders.Clear();

        //        _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PT_I, Permissao.PT_C }, "7777710", "7777710", "7777710", "40E1E074-37D6-E911-ABD6-F81654FE895D"));

        //        PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto = CriarDtoPlanoAnualTerritorioSaber();

        //        var jsonParaPost = new StringContent(SgpJsonSerializer.Serialize(planoAnualTerritorioSaberDto), Encoding.UTF8, "application/json");

        //        var postResult = _fixture._clientApi.PostAsync("api/v1/planos/anual/territorio-saber/", jsonParaPost).Result;

        //        Assert.True(postResult.IsSuccessStatusCode);

        //        var planoAnualTerritorioSaberCompletoResponse = _fixture._clientApi.
        //            GetAsync($@"api/v1/planos/anual/territorio-saber?
        //                    turmaId={planoAnualTerritorioSaberDto.TurmaId}&
        //                    ueId={planoAnualTerritorioSaberDto.EscolaId}&
        //                    anoLetivo={planoAnualTerritorioSaberDto.AnoLetivo}&
        //                    territorioExperienciaId={planoAnualTerritorioSaberDto.TerritorioExperienciaId}").Result;
        //        if (planoAnualTerritorioSaberCompletoResponse.IsSuccessStatusCode)
        //        {
        //            var planoAnualCompleto = SgpJsonSerializer.Deserialize<PlanoAnualTerritorioSaberCompletoDto>(planoAnualTerritorioSaberCompletoResponse.Content.ReadAsStringAsync().Result);
        //            Assert.Contains(planoAnualTerritorioSaberDto.Bimestres, c => c.Desenvolvimento == planoAnualCompleto.Desenvolvimento && 
        //                                                                         c.Reflexao == planoAnualCompleto.Reflexao);
        //        }
        //        else
        //        {
        //            var erro = postResult.Content.ReadAsStringAsync().Result;
        //            Assert.True(false, erro);
        //        }
        //    }
        //    catch (AggregateException ae)
        //    {
        //        throw new Exception("Erros: " + string.Join(",", ae.InnerExceptions));
        //    }
        //}

        [Fact, Order(4)]
        public void NaoDeveIncluirPlanoAnualTerritorioSaberEExibirMensagemErro()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PT_I, Permissao.PT_C }));

            PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto = CriarDtoPlanoAnualTerritorioSaber();
            planoAnualTerritorioSaberDto.EscolaId = null;
            var jsonParaPost = new StringContent(SgpJsonSerializer.Serialize(planoAnualTerritorioSaberDto), Encoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/planos/anual/territorio-saber/", jsonParaPost).Result;

            Assert.False(postResult.IsSuccessStatusCode);
            var jsonErro = postResult.Content.ReadAsStringAsync().Result;
            var retornoBase = SgpJsonSerializer.Deserialize<RetornoBaseDto>(jsonErro);
            Assert.Contains(retornoBase.Mensagens, c => c.Equals("A escola deve ser informada"));
        }

        private static PlanoAnualTerritorioSaberDto CriarDtoPlanoAnualTerritorioSaber()
        {
            return new PlanoAnualTerritorioSaberDto()
            {
                AnoLetivo = 2019,
                EscolaId = "095346",
                TurmaId = 2008187,
                TerritorioExperienciaId = 1242132143125,
                Bimestres = new List<BimestrePlanoAnualTerritorioSaberDto>
                {
                    new BimestrePlanoAnualTerritorioSaberDto
                    {
                        Bimestre = 1,
                        Desenvolvimento = "Descricao",
                        Reflexao = "Reflexao"
                    }
                },
            };
        }
    }
}
