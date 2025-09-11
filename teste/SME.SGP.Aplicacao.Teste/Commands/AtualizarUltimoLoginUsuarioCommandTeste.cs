using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands
{
    public class AtualizarUltimoLoginUsuarioCommandTeste
    {
        private readonly Mock<IRepositorioUsuario> repositorioMock;
        private readonly AtualizarUltimoLoginUsuarioCommandHandler handler;

        public AtualizarUltimoLoginUsuarioCommandTeste()
        {
            repositorioMock = new Mock<IRepositorioUsuario>();
            handler = new AtualizarUltimoLoginUsuarioCommandHandler(repositorioMock.Object);
        }

        [Fact(DisplayName = "Handle deve atualizar último login e retornar true")]
        public async Task Handle_Deve_Atualizar_Ultimo_Login_E_Retornar_True()
        {
            var usuario = new Usuario { Id = 1, UltimoLogin = DateTime.UtcNow };
            var command = new AtualizarUltimoLoginUsuarioCommand(usuario);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.True(resultado);
            repositorioMock.Verify(r => r.AtualizarUltimoLogin(usuario.Id, usuario.UltimoLogin), Times.Once);
        }

        [Fact(DisplayName = "Construtor lança ArgumentNullException quando repositorio é nulo")]
        public void Construtor_Deve_Lancar_Argument_Null_Exception_Quando_Repositorio_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new AtualizarUltimoLoginUsuarioCommandHandler(null));
        }
    }
}
