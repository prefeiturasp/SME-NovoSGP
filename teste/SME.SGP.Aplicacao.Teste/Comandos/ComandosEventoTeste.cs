using Moq;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosEventoTeste
    {
        private readonly ComandosEvento comandosEvento;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioEventoTipo> repositorioEventoTipo;
        private readonly Mock<IServicoEvento> servicoEvento;
        private readonly Mock<IServicoWorkflowAprovacao> servicoWorkflowAprovacao;

        public ComandosEventoTeste()
        {
            repositorioEvento = new Mock<IRepositorioEvento>();
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            servicoEvento = new Mock<IServicoEvento>();
            servicoWorkflowAprovacao = new Mock<IServicoWorkflowAprovacao>();
            comandosEvento = new ComandosEvento(repositorioEvento.Object, repositorioEventoTipo.Object, servicoEvento.Object, servicoWorkflowAprovacao.Object);
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