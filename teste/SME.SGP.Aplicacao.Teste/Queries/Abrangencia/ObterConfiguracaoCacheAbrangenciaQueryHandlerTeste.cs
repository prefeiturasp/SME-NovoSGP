using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Abrangencia
{
    public class ObterConfiguracaoCacheAbrangenciaQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterConfiguracaoCacheAbrangenciaQueryHandler handler;

        public ObterConfiguracaoCacheAbrangenciaQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            handler = new ObterConfiguracaoCacheAbrangenciaQueryHandler(mediator.Object, new MemoryCache(new MemoryCacheOptions()));
        }

        private void ValorDoParametro(string valor)
            => mediator.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoQuery>(q => q.Tipo == TipoParametroSistema.UtilizarCacheAbrangencia), It.IsAny<CancellationToken>()))
                .ReturnsAsync(valor);

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Parametro_Inativo()
        {
            //Arrange
            ValorDoParametro(null);

            //Act
            var retorno = await handler.Handle(ObterConfiguracaoCacheAbrangenciaQuery.Instance, CancellationToken.None);

            //Assert
            Assert.Null(retorno);
        }

        [Fact]
        public async Task Deve_Ler_Os_Ttls_Do_Json()
        {
            //Arrange
            ValorDoParametro("{\"minutos\":3,\"cj\":2}");

            //Act
            var retorno = await handler.Handle(ObterConfiguracaoCacheAbrangenciaQuery.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(3, retorno.Minutos);
            Assert.Equal(2, retorno.Cj);
            Assert.Equal(2, retorno.ObterMinutosExpiracao(Perfis.PERFIL_CJ_INFANTIL));
            Assert.Equal(3, retorno.ObterMinutosExpiracao(Perfis.PERFIL_PROFESSOR));
        }

        [Theory]
        [InlineData("")]
        [InlineData("isso nao e json")]
        [InlineData("{\"minutos\":0,\"cj\":1}")]
        [InlineData("{\"minutos\":5,\"cj\":-1}")]
        public async Task Deve_Usar_Padrao_Quando_Json_Invalido(string valor)
        {
            //Arrange
            ValorDoParametro(valor);

            //Act
            var retorno = await handler.Handle(ObterConfiguracaoCacheAbrangenciaQuery.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(ConfiguracaoCacheAbrangenciaDto.MINUTOS_PADRAO, retorno.Minutos);
            Assert.Equal(ConfiguracaoCacheAbrangenciaDto.MINUTOS_CJ_PADRAO, retorno.Cj);
        }

        [Fact]
        public async Task Deve_Ficar_Desligado_Quando_Leitura_Do_Parametro_Falhar()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("banco fora"));

            //Act
            var retorno = await handler.Handle(ObterConfiguracaoCacheAbrangenciaQuery.Instance, CancellationToken.None);

            //Assert
            Assert.Null(retorno);
        }

        [Fact]
        public async Task Deve_Ler_O_Parametro_Uma_Vez_E_Guardar_Em_Memoria()
        {
            //Arrange
            ValorDoParametro(null);

            //Act
            await handler.Handle(ObterConfiguracaoCacheAbrangenciaQuery.Instance, CancellationToken.None);
            await handler.Handle(ObterConfiguracaoCacheAbrangenciaQuery.Instance, CancellationToken.None);
            await handler.Handle(ObterConfiguracaoCacheAbrangenciaQuery.Instance, CancellationToken.None);

            //Assert
            mediator.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
