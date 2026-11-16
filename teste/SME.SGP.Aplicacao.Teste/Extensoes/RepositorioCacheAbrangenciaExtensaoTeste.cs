using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Extensoes
{
    public class RepositorioCacheAbrangenciaExtensaoTeste
    {
        [Fact]
        public void Deve_Gerar_Mesma_Chave_Para_Listas_Em_Ordem_Diferente()
        {
            //Arrange
            var a = new[] { 22, 14, 19 };
            var b = new[] { 19, 22, 14 };

            //Act & Assert
            Assert.Equal("14,19,22", a.ParaChaveCache());
            Assert.Equal(a.ParaChaveCache(), b.ParaChaveCache());
        }

        [Fact]
        public void Deve_Diferenciar_Lista_Nula_De_Lista_Vazia()
        {
            //Act & Assert
            Assert.Equal("null", ((IEnumerable<int>)null).ParaChaveCache());
            Assert.Equal(string.Empty, Array.Empty<int>().ParaChaveCache());
        }

        [Fact]
        public async Task Deve_Chamar_O_Banco_Uma_Vez_Quando_Json_Do_Cache_Estiver_Corrompido()
        {
            //Arrange
            var cache = new Mock<IRepositorioCache>();
            cache.Setup(c => c.ObterAsync(It.IsAny<string>(), false)).ReturnsAsync("{corrompido");
            var chamadasBanco = 0;

            //Act
            var retorno = await cache.Object.ObterComCacheAsync("chave", () => { chamadasBanco++; return Task.FromResult(new[] { 2026 }); }, 5);

            //Assert
            Assert.Equal(1, chamadasBanco);
            Assert.Equal(new[] { 2026 }, retorno);
        }

        [Fact]
        public async Task Deve_Chamar_O_Banco_Uma_Vez_Quando_Leitura_Do_Cache_Falhar()
        {
            //Arrange
            var cache = new Mock<IRepositorioCache>();
            cache.Setup(c => c.ObterAsync(It.IsAny<string>(), false)).ThrowsAsync(new Exception("redis fora"));
            var chamadasBanco = 0;

            //Act
            await cache.Object.ObterComCacheAsync("chave", () => { chamadasBanco++; return Task.FromResult(new[] { 2026 }); }, 5);

            //Assert
            Assert.Equal(1, chamadasBanco);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Do_Banco_Mesmo_Quando_Gravacao_No_Cache_Falhar()
        {
            //Arrange
            var cache = new Mock<IRepositorioCache>();
            cache.Setup(c => c.ObterAsync(It.IsAny<string>(), false)).ReturnsAsync(string.Empty);
            cache.Setup(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("redis fora"));

            //Act
            var retorno = await cache.Object.ObterComCacheAsync("chave", () => Task.FromResult(new[] { 2026 }), 5);

            //Assert
            Assert.Equal(new[] { 2026 }, retorno);
        }

        [Fact]
        public void Deve_Manter_Ue_Igual_Apos_Serializar_E_Desserializar()
        {
            //Arrange
            var ue = new AbrangenciaUeRetorno { Id = 1, Codigo = "019769", NomeSimples = "MARINA COLASANTI", TipoEscola = TipoEscola.EMEF };

            //Act
            var retorno = JsonConvert.DeserializeObject<AbrangenciaUeRetorno>(JsonConvert.SerializeObject(ue));

            //Assert
            Assert.Equal(ue.Nome, retorno.Nome);
            Assert.Equal(ue.EhInfantil, retorno.EhInfantil);
            Assert.Equal(ue.Codigo, retorno.Codigo);
            Assert.Equal(ue.Id, retorno.Id);
        }
    }
}
