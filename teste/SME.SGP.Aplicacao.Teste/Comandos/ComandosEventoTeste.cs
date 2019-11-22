using Moq;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosEventoTeste
    {
        private readonly ComandosEvento comandosEvento;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IServicoDiaLetivo> servicoDiaLetivo;
        private readonly Mock<IServicoEvento> servicoEvento;

        public ComandosEventoTeste()
        {
            repositorioEvento = new Mock<IRepositorioEvento>();
            servicoEvento = new Mock<IServicoEvento>();
            servicoDiaLetivo = new Mock<IServicoDiaLetivo>();
            comandosEvento = new ComandosEvento(repositorioEvento.Object, servicoEvento.Object, servicoDiaLetivo.Object);
        }

        [Fact]
        public void Deve_Excluir_Eventos()
        {
            //ARRANGE
            var evento1 = new Dominio.Evento() { Id = 1 };
            repositorioEvento.Setup(a => a.ObterPorId(1)).Returns(evento1);

            //ACT
            comandosEvento.Excluir(new long[] { 1 });

            //ASSERT
            repositorioEvento.Verify(a => a.ObterPorId(evento1.Id), Times.Once);
            repositorioEvento.Verify(a => a.Salvar(evento1), Times.Once);
        }
    }
}