using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IServicoAbrangencia> servicoAbrangencia;

        public ComandosEventoTeste()
        {
            repositorioEvento = new Mock<IRepositorioEvento>();
            repositorioEventoTipo = new Mock<IRepositorioEventoTipo>();
            servicoEvento = new Mock<IServicoEvento>();
            servicoWorkflowAprovacao = new Mock<IServicoWorkflowAprovacao>();
            servicoUsuario = new Mock<IServicoUsuario>();
            servicoAbrangencia = new Mock<IServicoAbrangencia>();
            comandosEvento = new ComandosEvento(repositorioEvento.Object, repositorioEventoTipo.Object, servicoEvento.Object, servicoWorkflowAprovacao.Object, servicoUsuario.Object, servicoAbrangencia.Object);
        }

        [Fact]
        public void Deve_Excluir_Eventos()
        {
            //ARRANGE
            var evento1 = new Evento() { Id = 1 };
            repositorioEvento.Setup(a => a.ObterPorId(1)).Returns(evento1);
            var usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { Tipo = TipoPerfil.SME } });
            servicoUsuario.Setup(u => u.ObterUsuarioLogado()).Returns(Task.FromResult(usuario));

            //ACT
            comandosEvento.Excluir(new long[] { 1 });

            //ASSERT
            repositorioEvento.Verify(a => a.ObterPorId(evento1.Id), Times.Once);
            repositorioEvento.Verify(a => a.Salvar(evento1), Times.Once);
        }
    }
}