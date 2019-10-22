using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class EventoTipoTeste
    {
        private readonly Func<string> obterUrlDelete = () => "api/v1/evento/tipo";
        private readonly Func<int, string> obterUrlGet = (codigo) => $"api/v1/evento/tipo/{codigo}";
        private readonly Func<string> obterUrlListar = () => "api/v1/evento/tipo/Listar";
        private readonly Func<string> obterUrlPost = () => "api/v1/evento/tipo";
        private TestServerFixture _fixture;

        public EventoTipoTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Deve_Cadastar_Apagar_Tipo_Evento"), Order(4)]
        public void Deve_Cadastar_Apagar_Tipo_Evento()
        {
            _fixture = ObtenhaCabecalho();

            var postResult = TesteBase.ExecutePostAsync(_fixture, obterUrlPost(), ObtenhaObjetoCadastrar());

            Assert.True(postResult.IsSuccessStatusCode);

            var deleteResult = TesteBase.ExecuteDeleteAsync(_fixture, obterUrlDelete(), new List<long> { 1 });

            Assert.True(deleteResult.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Cadastar_Editar_Tipo_Evento"), Order(3)]
        public void Deve_Cadastar_Editar_Tipo_Evento()
        {
            _fixture = ObtenhaCabecalho();

            var Dto = ObtenhaObjetoCadastrar();

            var postResult = TesteBase.ExecutePostAsync(_fixture, obterUrlPost(), Dto);

            Assert.True(postResult.IsSuccessStatusCode);

            Dto.Codigo = 1;
            Dto.Descricao = "Teste Integracao";

            var postResult2 = TesteBase.ExecutePostAsync(_fixture, obterUrlPost(), Dto);

            Assert.True(postResult2.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Cadastrar_Consultar_Tipo_Evento"), Order(2)]
        public async void Deve_Cadastrar_Consultar_Tipo_Evento()
        {
            _fixture = ObtenhaCabecalho();

            var Dto = ObtenhaObjetoCadastrar();

            var postResult = TesteBase.ExecutePostAsync(_fixture, obterUrlPost(), Dto);

            Assert.True(postResult.IsSuccessStatusCode);

            var getResult = TesteBase.ExecuteGetAsync(_fixture, obterUrlGet(1));

            Assert.True(getResult.IsSuccessStatusCode);

            Assert.IsType<EventoTipoDto>(JsonConvert.DeserializeObject<EventoTipoDto>(await getResult.Content.ReadAsStringAsync()));
        }

        [Fact(DisplayName = "Deve_Consultar_Retorno_Vazio"), Order(1)]
        public async Task Deve_Consultar_Retorno_Vazio()
        {
            _fixture = ObtenhaCabecalho();

            var getResult = TesteBase.ExecuteGetAsync(_fixture, obterUrlGet(1));

            Assert.True(getResult.IsSuccessStatusCode);

            Assert.IsNotType<EventoTipoDto>(JsonConvert.DeserializeObject<EventoTipoDto>(await getResult.Content.ReadAsStringAsync()));
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Se_Tentativa_Delete_Sem_Tipo_Cadastrado"), Order(5)]
        public void Deve_Disparar_Excecao_Se_Tentativa_Delete_Sem_Tipo_Cadastrado()
        {
            _fixture = ObtenhaCabecalho();

            var deleteResult = TesteBase.ExecuteDeleteAsync(_fixture, obterUrlDelete(), new List<long> { 10, 11, 12 });

            Assert.False(deleteResult.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Listar_Evento_Tipo"), Order(7)]
        public void Deve_Listar_Evento_Tipo()
        {
            _fixture = ObtenhaCabecalho();

            FiltroEventoTipoDto Dto = new FiltroEventoTipoDto();

            var postResult = TesteBase.ExecutePostAsync(_fixture, obterUrlListar(), Dto);

            Assert.True(postResult.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Salvar_Evento_Tipo"), Order(6)]
        public void Deve_Salvar_Evento_Tipo()
        {
            _fixture = ObtenhaCabecalho();

            EventoTipoDto Dto = ObtenhaObjetoCadastrar();

            var postResult = TesteBase.ExecutePostAsync(_fixture, obterUrlPost(), Dto);

            Assert.True(postResult.IsSuccessStatusCode);
        }

        private static EventoTipoDto ObtenhaObjetoCadastrar()
        {
            return new EventoTipoDto
            {
                Ativo = true,
                Concomitancia = true,
                Dependencia = false,
                Letivo = EventoLetivo.Opcional,
                LocalOcorrencia = EventoLocalOcorrencia.Todos,
                Descricao = "Teste 123",
                TipoData = EventoTipoData.Unico
            };
        }

        private TestServerFixture ObtenhaCabecalho()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = TesteBase.ObtenhaCabecalhoAuthentication(_fixture, permissoes);

            return _fixture;
        }
    }
}