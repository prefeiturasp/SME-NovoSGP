using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class RegistroPoaTeste
    {
        private TestServerFixture fixture;
        private Func<int, string> obterUrlDelete = (id) => $"api/v1/atribuicao/poa/{id}";
        private Func<int, string> obterUrlGet = (id) => $"api/v1/atribuicao/poa/{id}";
        private Func<string> obterUrlPost = () => "api/v1/atribuicao/poa";
        private Func<int, string> obterUrlPut = (id) => $"api/v1/atribuicao/poa/{id}";

        public RegistroPoaTeste(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        //[Fact(DisplayName = "Deve_Cadastrar_Registro_POA"), Order(1)]
        //public void Deve_Cadastrar_Registro_POA()
        //{
        //    fixture = ObtenhaCabecalho(inclusao: true);

        //    var postResult = TesteBase.ExecutePostAsync(fixture, obterUrlPost(), ObtenhaObjetoEnvio());

        //    Assert.True(postResult.IsSuccessStatusCode, postResult.Content.ReadAsStringAsync().Result);
        //}

        //[Fact(DisplayName = "Deve_Editar_Registro_POA"), Order(3)]
        //public void Deve_Editar_Registro_POA()
        //{
        //    fixture = ObtenhaCabecalho(alteracao: true);

        //    var postResult = TesteBase.ExecutePutAsync(fixture, obterUrlPut(1), ObtenhaObjetoEnvio());

        //    Assert.True(postResult.IsSuccessStatusCode, postResult.Content.ReadAsStringAsync().Result);
        //}

        //[Fact(DisplayName = "Deve_Excluir_Registro_POA"), Order(4)]
        //public void Deve_Excluir_Registro_POA()
        //{
        //    fixture = ObtenhaCabecalho(exclusao: true);

        //    var postResult = TesteBase.ExecuteDeleteAsync(fixture, obterUrlDelete(1));

        //    Assert.True(postResult.IsSuccessStatusCode, postResult.Content.ReadAsStringAsync().Result);
        //}

        //[Fact(DisplayName = "Deve_Obter_Registro_Por_Id"), Order(2)]
        //public void Deve_Obter_Registro_Por_Id()
        //{
        //    fixture = ObtenhaCabecalho(consulta: true);

        //    var postResult = TesteBase.ExecutePostAsync(fixture, obterUrlGet(1), ObtenhaObjetoEnvio());

        //    Assert.True(postResult.IsSuccessStatusCode, postResult.Content.ReadAsStringAsync().Result);
        //    Assert.Equal(HttpStatusCode.OK, postResult.StatusCode);
        //}

        private TestServerFixture ObtenhaCabecalho(bool inclusao = false, bool alteracao = false, bool consulta = false, bool exclusao = false)
        {
            var permissoes = ObterPermissionamentos(inclusao, alteracao, consulta, exclusao);

            fixture = TesteBase.ObtenhaCabecalhoAuthentication(fixture, permissoes, "8246432", "8246432", "3fe1e074-37d6-e911-abd6-f81654fe895d");

            return fixture;
        }

        private RegistroPoaDto ObtenhaObjetoEnvio()
        {
            return new RegistroPoaDto
            {
                AnoLetivo = DateTime.Now.Year,
                CodigoRf = "7777710",
                Descricao = "Teste 123",
                DreId = "1",
                Bimestre = DateTime.Now.Month,
                Titulo = "Teste integrado",
                UeId = "1"
            };
        }

        private Permissao[] ObterPermissionamentos(bool inclusao, bool alteracao, bool consulta, bool exclusao)
        {
            var permissoes = new List<Permissao>();

            if (inclusao) permissoes.Add(Permissao.RPOA_I);
            if (alteracao) permissoes.Add(Permissao.RPOA_A);
            if (consulta) permissoes.Add(Permissao.RPOA_C);
            if (exclusao) permissoes.Add(Permissao.RPOA_E);

            return permissoes.ToArray();
        }
    }
}