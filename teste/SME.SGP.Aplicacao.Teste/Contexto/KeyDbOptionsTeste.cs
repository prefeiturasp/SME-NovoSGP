using SME.SGP.Infra.Utilitarios;
using StackExchange.Redis;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Contexto
{
    public class KeyDbOptionsTeste
    {
        private static RedisOptions RedisPrincipal()
            => new RedisOptions { Endpoint = "redis-principal:6379", Proxy = Proxy.Twemproxy, SyncTimeout = 3000, Prefixo = "SGP_" };

        [Fact]
        public void Deve_Usar_Redis_Principal_Quando_EndPoint_AB_Nao_Configurado()
        {
            //Arrange
            var keyDb = new KeyDbOptions();
            var principal = RedisPrincipal();

            //Act
            var opcoes = keyDb.ObterOpcoesAbrangencia(principal);

            //Assert
            Assert.Equal("redis-principal:6379", opcoes.Endpoint);
            Assert.Equal(Proxy.Twemproxy, opcoes.Proxy);
            Assert.Equal(3000, opcoes.SyncTimeout);
            Assert.Equal("SGP_", opcoes.Prefixo);
        }

        [Fact]
        public void Deve_Usar_KeyDB_Sem_Proxy_Quando_EndPoint_AB_Configurado()
        {
            //Arrange
            var keyDb = new KeyDbOptions { EndPoint_AB = "keydb-ab:6379", Prefixo_AB = "SGP_AB_", SyncTimeout_AB = 2000 };

            //Act
            var opcoes = keyDb.ObterOpcoesAbrangencia(RedisPrincipal());

            //Assert
            Assert.Equal("keydb-ab:6379", opcoes.Endpoint);
            Assert.Equal(Proxy.None, opcoes.Proxy);
            Assert.Equal(2000, opcoes.SyncTimeout);
            Assert.Equal("SGP_AB_", opcoes.Prefixo);
        }

        [Fact]
        public void Deve_Usar_Prefixo_E_SyncTimeout_Do_Principal_Quando_Nao_Informados_No_KeyDB()
        {
            //Arrange
            var keyDb = new KeyDbOptions { EndPoint_AB = "keydb-ab:6379" };

            //Act
            var opcoes = keyDb.ObterOpcoesAbrangencia(RedisPrincipal());

            //Assert
            Assert.Equal("keydb-ab:6379", opcoes.Endpoint);
            Assert.Equal(Proxy.None, opcoes.Proxy);
            Assert.Equal(3000, opcoes.SyncTimeout);
            Assert.Equal("SGP_", opcoes.Prefixo);
        }
    }
}
