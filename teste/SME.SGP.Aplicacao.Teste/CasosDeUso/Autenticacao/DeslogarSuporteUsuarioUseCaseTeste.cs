using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.Autenticacao.DeslogarSuporteUsuario;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Autenticacao
{
    public class DeslogarSuporteUsuarioUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DeslogarSuporteUsuarioUseCase _useCase;

        public DeslogarSuporteUsuarioUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new DeslogarSuporteUsuarioUseCase(_mediatorMock.Object);
        }

        private AdministradorSuporte CriarAdministrador(string login)
        {
            return new AdministradorSuporte { Nome = "Teste", Login = login };
        }

        private UsuarioAutenticacaoRetornoDto CriarRetornoSucesso()
        {
            return new UsuarioAutenticacaoRetornoDto { Autenticado = true };
        }

        [Fact]
        public async Task Executar_Quando_Administrador_Valido_Deve_Executar_Comando_E_Retornar_Sucesso()
        {
            var administrador = CriarAdministrador("admin.teste");
            var retornoEsperado = CriarRetornoSucesso();

            _mediatorMock.Setup(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(administrador);

            _mediatorMock.Setup(m => m.Send(It.Is<DeslogarSuporteUsuarioCommand>(c => c.Administrador == administrador), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar();

            Assert.NotNull(resultado);
            Assert.True(resultado.Autenticado);
            _mediatorMock.Verify(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<DeslogarSuporteUsuarioCommand>(c => c.Administrador == administrador), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Administrador_Eh_Nulo_Deve_Lancar_NegocioException()
        {
            AdministradorSuporte administradorNulo = null;

            _mediatorMock.Setup(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(administradorNulo);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar());
            _mediatorMock.Verify(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeslogarSuporteUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Administrador_Login_Vazio_Deve_Lancar_NegocioException()
        {
            var administradorLoginVazio = CriarAdministrador(string.Empty);

            _mediatorMock.Setup(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(administradorLoginVazio);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar());
            _mediatorMock.Verify(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeslogarSuporteUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Administrador_Login_Nulo_Deve_Lancar_NegocioException()
        {
            var administradorLoginNulo = CriarAdministrador(null);

            _mediatorMock.Setup(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(administradorLoginNulo);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar());
            _mediatorMock.Verify(m => m.Send(ObterAdministradorDoSuporteQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeslogarSuporteUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
