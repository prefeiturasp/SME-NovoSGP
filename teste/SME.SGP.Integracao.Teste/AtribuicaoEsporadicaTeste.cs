using SME.SGP.Api.Controllers;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class AtribuicaoEsporadicaTeste
    {
        private readonly Func<string> obterUrlPost = () => "api/v1/atribuicao/esporadica";
        private TestServerFixture _fixture;

        public AtribuicaoEsporadicaTeste(TestServerFixture fixture)
        {
            _fixture = fixture;

            TesteBase.AdicionarTipoCalendario(_fixture);
            TesteBase.AdicionarPeriodoEscolar(_fixture);
        }

        //TODO: Refatorar teste
        //[Fact(DisplayName = "Deve_Cadastrar_Atribuicao_Esporadica"), Order(1)]
        //public void Deve_Cadastrar_Atribuicao_Esporadica()
        //{
        //    _fixture = ObtenhaCabecalho(inclusao: true);

        //    var postResult = TesteBase.ExecutePostAsync(_fixture, obterUrlPost(), ObtenhaObjetoEnvio());

        //    Assert.True(postResult.IsSuccessStatusCode, postResult.Content.ReadAsStringAsync().Result);
        //}

        private TestServerFixture ObtenhaCabecalho(bool inclusao = false, bool alteracao = false, bool consulta = false, bool exclusao = false)
        {
            var permissoes = ObterPermissionamentos(inclusao, alteracao, consulta, exclusao);

            _fixture = TesteBase.ObtenhaCabecalhoAuthentication(_fixture, permissoes, "6951040", "6951040", "46e1e074-37d6-e911-abd6-f81654fe895d");

            return _fixture;
        }

        private AtribuicaoEsporadicaDto ObtenhaObjetoEnvio()
        {
            return new AtribuicaoEsporadicaDto
            {
                AnoLetivo = DateTime.Now.Year,
                DataFim = DateTime.Now.AddSeconds(10),
                DataInicio = DateTime.Now,
                DreId = "1",
                Id = 0,
                Excluido = false,
                Migrado = false,
                ProfessorNome = "Professor Teste",
                ProfessorRf = "00001",
                UeId = "1"
            };
        }

        private Permissao[] ObterPermissionamentos(bool inclusao, bool alteracao, bool consulta, bool exclusao)
        {
            var permissoes = new List<Permissao>();

            if (inclusao) permissoes.Add(Permissao.AE_I);
            if (alteracao) permissoes.Add(Permissao.AE_A);
            if (consulta) permissoes.Add(Permissao.AE_C);
            if (exclusao) permissoes.Add(Permissao.AE_E);

            return permissoes.ToArray();
        }
    }
}