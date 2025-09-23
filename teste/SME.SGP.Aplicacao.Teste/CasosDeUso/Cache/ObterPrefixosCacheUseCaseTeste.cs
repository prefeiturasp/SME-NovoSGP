using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Linq;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Cache
{
    public class ObterPrefixosCacheUseCaseTeste
    {
        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Config_For_Nulo()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ObterPrefixosCacheUseCase(null));
            Assert.Equal("config", ex.ParamName);
        }

        [Fact]
        public void Executar_Deve_Retornar_Prefixos_Com_Base_Nas_Constantes()
        {
            var config = new RedisOptions { Prefixo = "SGP_" };
            var useCase = new ObterPrefixosCacheUseCase(config);

            var resultado = useCase.Executar().ToList();

            Assert.NotEmpty(resultado);

            var constantes = typeof(NomeChaveCache).ObterConstantesPublicas<string>().ToList();

            Assert.Equal(constantes.Count, resultado.Count);

            foreach (var chave in constantes)
            {
                var prefixoEsperado = $"{config.Prefixo}{chave.Split(':')[0]}";
                Assert.Contains(prefixoEsperado, resultado);
            }
        }

        [Fact]
        public void Redis_Options_Deve_Ter_Valores_Padrao()
        {
            var options = new RedisOptions();

            Assert.Equal(5000, options.SyncTimeout);
            Assert.Equal("SGP_", options.Prefixo);

            options.Endpoint = "localhost:6379";
            options.Proxy = StackExchange.Redis.Proxy.Twemproxy;
            options.SyncTimeout = 10000;
            options.Prefixo = "CUSTOM_";

            Assert.Equal("localhost:6379", options.Endpoint);
            Assert.Equal(StackExchange.Redis.Proxy.Twemproxy, options.Proxy);
            Assert.Equal(10000, options.SyncTimeout);
            Assert.Equal("CUSTOM_", options.Prefixo);
        }

        [Fact]
        public void Nome_Chave_Cache_Deve_Conter_Constantes()
        {
            var constantes = typeof(NomeChaveCache).ObterConstantesPublicas<string>().ToList();

            Assert.NotEmpty(constantes);

            foreach (var c in constantes)
            {
                Assert.False(string.IsNullOrWhiteSpace(c));
            }
        }
    }
}
