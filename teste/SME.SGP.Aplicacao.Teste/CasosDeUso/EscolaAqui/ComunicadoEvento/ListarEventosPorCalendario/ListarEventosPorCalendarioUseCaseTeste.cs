using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.ComunicadoEvento.ListarEventosPorCalendario
{
    public class ListarEventosPorCalendarioUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ListarEventosPorCalendarioUseCase useCase;

        public ListarEventosPorCalendarioUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ListarEventosPorCalendarioUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Com_Sucesso_E_Retornar_Eventos()
        {
            var parametros = new ListarEventoPorCalendarioDto
            {
                TipoCalendario = 1,
                AnoLetivo = 2025,
                CodigoDre = "DRE01",
                CodigoUe = "UE01",
                Modalidades = new List<int> { 1, 2 }
            };

            var retornoEsperado = new List<EventoCalendarioRetornoDto>
            {
                new EventoCalendarioRetornoDto { Id = 1, Nome = "Evento 1" },
                new EventoCalendarioRetornoDto { Id = 2, Nome = "Evento 2" }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ListarEventosPorCalendarioQuery>(
                    q =>
                        q.TipoCalendario == parametros.TipoCalendario &&
                        q.AnoLetivo == parametros.AnoLetivo &&
                        q.CodigoDre == parametros.CodigoDre &&
                        q.CodigoUe == parametros.CodigoUe &&
                        q.Modalidades.SequenceEqual(parametros.Modalidades)
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await useCase.Executar(parametros);

            Assert.Equal(retornoEsperado.Count, resultado.Count());
            Assert.Equal(retornoEsperado.First().Nome, resultado.First().Nome);
            mediatorMock.Verify(m => m.Send(It.IsAny<ListarEventosPorCalendarioQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
