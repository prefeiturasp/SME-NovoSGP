using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Abrangencia
{
    public class CarregarAbrangenciaUsuarioUseCaseTeste
    {
        [Fact]
        public async Task Deve_Enviar_Comando_Carregar_Abrangencia_Usuario()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new CarregarAbrangenciaUsusarioUseCase(mediatorMock.Object);

            var login = "5555555"; 
            var perfil = Guid.NewGuid();
            var usuarioPerfil = new UsuarioPerfilDto(login, perfil);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<CarregarAbrangenciaUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var resultado = await useCase.Executar(usuarioPerfil);

            mediatorMock.Verify(m => m.Send(
                It.Is<CarregarAbrangenciaUsuarioCommand>(c =>
                    c.Login == login && c.Perfil == perfil
                ),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(resultado);
        }
    }
}
