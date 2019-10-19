using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using System;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class EventoTipoTeste
    {
        private TestServerFixture _fixture;
        private Func<string> obterUrlPost = () => "api/v1/evento/tipo";
        private Func<int, string> obterUrlDelete = (codigo) => $"api/v1/evento/tipo/{codigo}";
        private Func<int, string> obterUrlGet = (codigo) => $"api/v1/evento/tipo/{codigo}";
        private Func<string> obterUrlListar = () => "api/v1/evento/tipo/Listar";

        public EventoTipoTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Deve_Listar_Evento_Tipo"),Order(7)]
        public void Deve_Listar_Evento_Tipo()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = ObtenhaCabecalho();

            FiltroEventoTipoDto Dto = new FiltroEventoTipoDto();

            var postResult = TesteBase.ExecutePostAsync(Dto, _fixture, obterUrlListar());

            Assert.True(postResult.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Salvar_Evento_Tipo"), Order(6)]
        public void Deve_Salvar_Evento_Tipo()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = ObtenhaCabecalho();

            EventoTipoDto Dto = ObtenhaObjetoCadastrar();

            var postResult = TesteBase.ExecutePostAsync(Dto, _fixture, obterUrlPost());

            Assert.True(postResult.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Disparar_Excecao_Se_Tentativa_Delete_Sem_Tipo_Cadastrado"), Order(5)]
        public void Deve_Disparar_Excecao_Se_Tentativa_Delete_Sem_Tipo_Cadastrado()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = ObtenhaCabecalho();

            var deleteResult = TesteBase.ExecuteDeleteAsync(_fixture, obterUrlDelete(1));

            Assert.False(deleteResult.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Cadastar_Apagar_Tipo_Evento"), Order(4)]
        public void Deve_Cadastar_Apagar_Tipo_Evento()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = ObtenhaCabecalho();

            var postResult = TesteBase.ExecutePostAsync(ObtenhaObjetoCadastrar(), _fixture, obterUrlPost());

            Assert.True(postResult.IsSuccessStatusCode);

            var deleteResult = TesteBase.ExecuteDeleteAsync(_fixture, obterUrlDelete(1));

            Assert.True(postResult.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Cadastar_Editar_Tipo_Evento"), Order(3)]
        public void Deve_Cadastar_Editar_Tipo_Evento()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = ObtenhaCabecalho();

            var Dto = ObtenhaObjetoCadastrar();

            var postResult = TesteBase.ExecutePostAsync(Dto, _fixture, obterUrlPost());

            Assert.True(postResult.IsSuccessStatusCode);

            Dto.Codigo = 1;
            Dto.Descricao = "Teste Integracao";

            var postResult2 = TesteBase.ExecutePostAsync(Dto, _fixture, obterUrlPost());

            Assert.True(postResult2.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Deve_Cadastrar_Consultar_Tipo_Evento"), Order(2)]
        public async void Deve_Cadastrar_Consultar_Tipo_Evento()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = ObtenhaCabecalho();

            var Dto = ObtenhaObjetoCadastrar();

            var postResult = TesteBase.ExecutePostAsync(Dto, _fixture, obterUrlPost());

            Assert.True(postResult.IsSuccessStatusCode);

            var getResult = TesteBase.ExecuteGetAsync(_fixture, obterUrlGet(1));

            Assert.True(getResult.IsSuccessStatusCode);
                        
            Assert.IsType<EventoTipoDto>(JsonConvert.DeserializeObject<EventoTipoDto>(await getResult.Content.ReadAsStringAsync()));
        }

        [Fact(DisplayName = "Deve_Consultar_Retorno_Vazio"), Order(1)]
        public async void Deve_Consultar_Retorno_Vazio()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = ObtenhaCabecalho();

            var getResult = TesteBase.ExecuteGetAsync(_fixture, obterUrlGet(1));

            Assert.True(getResult.IsSuccessStatusCode);

            Assert.IsNotType<EventoTipoDto>(JsonConvert.DeserializeObject<EventoTipoDto>(await getResult.Content.ReadAsStringAsync()));
        }

        private TestServerFixture ObtenhaCabecalho()
        {
            var permissoes = new Permissao[] { Permissao.PA_I };

            _fixture = TesteBase.ObtenhaCabecalhoAuthentication(_fixture, permissoes);

            return _fixture;
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

    }
}
