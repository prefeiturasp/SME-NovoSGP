using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries.MotivosAusencia.ObterMotivosAusencia;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.MotivosAusencia
{
    public class ObterMotivosAusenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterMotivosAusenciaUseCase useCase;

        public ObterMotivosAusenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterMotivosAusenciaUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new ObterMotivosAusenciaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retorna_Dados_Deve_Mapear_Para_Dto_Corretamente()
        {
            var listaMotivos = new List<MotivoAusencia>
            {
                new MotivoAusencia { Id = 1, Descricao = "Motivo 1" },
                new MotivoAusencia { Id = 2, Descricao = "Motivo 2" }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivosAusenciaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaMotivos);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("1", resultado.First().Valor);
            Assert.Equal("Motivo 1", resultado.First().Descricao);
            Assert.Equal("2", resultado.Last().Valor);
            Assert.Equal("Motivo 2", resultado.Last().Descricao);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retorna_Lista_Vazia_Deve_Retornar_Lista_Vazia()
        {
            var listaMotivos = new List<MotivoAusencia>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivosAusenciaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaMotivos);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retorna_Nulo_Deve_Retornar_Nulo()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivosAusenciaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((IEnumerable<MotivoAusencia>)null);

            var resultado = await useCase.Executar();

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retorna_Lista_Com_Item_Nulo_Deve_Mapear_Item_Como_Nulo()
        {
            var listaMotivos = new List<MotivoAusencia>
            {
                new MotivoAusencia { Id = 1, Descricao = "Motivo 1" },
                null
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMotivosAusenciaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaMotivos);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.NotNull(resultado.First());
            Assert.Null(resultado.Last());
        }
    }
}
