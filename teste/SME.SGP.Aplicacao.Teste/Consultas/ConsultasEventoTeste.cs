using Moq;
using SME.SGP.Aplicacao.Consultas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasEventoTeste
    {
        private readonly ConsultasEvento consultaEventos;
        private readonly Mock<IRepositorioEvento> repositorioEvento;

        public ConsultasEventoTeste()
        {
            repositorioEvento = new Mock<IRepositorioEvento>();
            consultaEventos = new ConsultasEvento(repositorioEvento.Object);
        }

        [Fact]
        public void DeveObterEvento()
        {
            repositorioEvento.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(new Evento
                {
                    Id = 1
                });
            var eventoDto = consultaEventos.ObterPorId(1);
            Assert.NotNull(eventoDto);
            Assert.Equal(1, eventoDto.Id);
            repositorioEvento.Verify(c => c.ObterPorId(It.IsAny<long>()), Times.Once);
        }
    }
}