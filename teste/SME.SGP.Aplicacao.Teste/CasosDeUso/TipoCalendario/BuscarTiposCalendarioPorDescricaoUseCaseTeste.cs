using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.TipoCalendario
{
    public class BuscarTiposCalendarioPorDescricaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly BuscarTiposCalendarioPorDescricaoUseCase buscarTiposCalendarioPorDescricaoUseCaseTeste;

        public BuscarTiposCalendarioPorDescricaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            buscarTiposCalendarioPorDescricaoUseCaseTeste = new BuscarTiposCalendarioPorDescricaoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Lista_Tipo_Calendario_Pela_Busca()
        {
            //Arrange
            var mockRetorno = new List<TipoCalendarioBuscaDto> {
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário Fundamental",
                        Id = 1,
                        Nome = "Calendário Fundamental"
                    },
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário Médio",
                        Id = 1,
                        Nome = "Calendário Médio"
                    },
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário EJA",
                        Id = 1,
                        Nome = "Calendário EJA"
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ObterTipoCalendarioPorBuscaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);

            //Act
            var retorno = await buscarTiposCalendarioPorDescricaoUseCaseTeste.Executar("2020");

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterTipoCalendarioPorBuscaQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }
    }
}
